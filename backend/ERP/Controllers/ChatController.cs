using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AIService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Core.Entities.ERPDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _systemPrompt = @"Base de données schema:
Tables:
Table : ""Article"" (et non ""Articles"")
Colonnes :
- ""idArt"" (Clé primaire)
- ""designation"" (Le nom de l'article)
- ""description"" 
- ""prixUnitaire""
- ""stockDispo"" (Au lieu de StockRestant)
- ""dateAjout""
- ""imageUrl""
- ""idScat"" (ID Sous-catégorie)



Instructions strictes:
- Si la question concerne les données de la base (récupération, filtrage, agrégation), tu dois répondre UNIQUEMENT par la requête SQL brute commençant par 'SELECT'. Ne fournis AUCUN texte explicatif, commentaire ou balisage.
- La requête doit utiliser les noms de colonnes et tables tels qu'ils sont décrits ci-dessus.
- N'effectue aucune modification, INSERT, UPDATE ou DELETE.
- Si la question ne concerne pas les données (conseils, explications, debug), répond normalement en texte clair.";

        public ChatController(IConfiguration configuration, Core.Entities.ERPDbContext context, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }



        private bool IsListRequest(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return false;
            var m = message.ToLowerInvariant();
            string[] keywords = new[] { "liste", "liste de", "list", "all", "tous", "toutes", "tous les", "affiche", "afficher", "montre", "show", "donne", "donnez", "récupère", "recuperer", "tous les" };
            return keywords.Any(k => m.Contains(k));
        }

        private string CleanSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return string.Empty;

            // Remove fenced code blocks like ```sql or ```
            sql = Regex.Replace(sql, "```(?:\\w+)?", string.Empty, RegexOptions.IgnoreCase);
            // Remove inline backticks
            sql = sql.Replace("`", string.Empty);
            // Trim and remove trailing semicolons and excessive whitespace
            sql = sql.Trim();
            sql = sql.TrimEnd(';');
            return Regex.Replace(sql, "\r?\n\\s+", " ").Trim();
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
                return BadRequest("Le message ne peut pas être vide.");


            try
            {
                // Read Mistral config
                var mistralSection = _configuration.GetSection("Mistral");
                var apiKey = mistralSection["ApiKey"] ?? Environment.GetEnvironmentVariable("MISTRAL_API_KEY");
                var endpoint = mistralSection["Endpoint"] ?? "https://api.mistral.ai/v1/generate";
                var model = mistralSection["Model"] ?? "mistral-small";

                if (string.IsNullOrEmpty(apiKey))
                    return StatusCode(500, new { error = "Mistral API key not configured. Set Mistral:ApiKey in configuration or the MISTRAL_API_KEY environment variable." });

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var payload = new
                {
                    model = model,
                    messages = new[] {
                        new { role = "system", content = _systemPrompt },
                        new { role = "user", content = request.Message }
                    },
                    temperature = 0.1
                };

                using var httpResponse = await client.PostAsJsonAsync(endpoint, payload);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var err = await httpResponse.Content.ReadAsStringAsync();
                    return StatusCode(502, new { error = "Mistral API error", details = err });
                }

                var respText = await httpResponse.Content.ReadAsStringAsync();
                string text = string.Empty;
                try
                {
                    using var doc = JsonDocument.Parse(respText);
                    if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                    {
                        var first = choices[0];
                        if (first.TryGetProperty("message", out var message) && message.TryGetProperty("content", out var content))
                        {
                            text = content.GetString() ?? string.Empty;
                        }
                    }
                }
                catch (JsonException)
                {
                    // Fallback: use raw response as text
                    text = respText;
                }

                // If the model returned a SQL SELECT, execute it against the database
                if (text.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                {
                    var cleanedSql = CleanSql(text);

                    try
                    {
                        var results = new List<Dictionary<string, object>>();
                        var connection = _context.Database.GetDbConnection();

                        // Sécurité : s'assurer que la connexion est bien ouverte
                        if (connection.State != System.Data.ConnectionState.Open)
                            await connection.OpenAsync();

                        try
                        {
                            using var command = connection.CreateCommand();
                            // If the user asked for a list, ensure we limit rows to protect large tables
                            if (IsListRequest(request.Message))
                            {
                                if (!Regex.IsMatch(cleanedSql, "\\bLIMIT\\b", RegexOptions.IgnoreCase) &&
                                    !Regex.IsMatch(cleanedSql, "\\bFETCH\\b", RegexOptions.IgnoreCase) &&
                                    !Regex.IsMatch(cleanedSql, "\\bTOP\\b", RegexOptions.IgnoreCase))
                                {
                                    cleanedSql = cleanedSql + " LIMIT 50";
                                }
                            }
                            command.CommandText = cleanedSql;

                            // Timeout : Pour éviter qu'une requête lourde sur 2M de lignes ne bloque le serveur
                            command.CommandTimeout = 30;

                            using var reader = await command.ExecuteReaderAsync();

                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var name = reader.GetName(i);
                                    // Récupération sécurisée de la valeur
                                    var value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                                    row[name] = value ?? "N/A";
                                }
                                results.Add(row);
                            }
                        }
                        finally
                        {
                            // Libérer la connexion immédiatement après lecture
                            await connection.CloseAsync();
                        }

                        // Retourner un objet clair pour ton composant Angular
                        return Ok(new
                        {
                            answer = results.Count > 0
                                ? "Voici les résultats trouvés dans la base de données :"
                                : "Aucune donnée ne correspond à votre recherche.",
                            data = results,
                            count = results.Count
                        });
                    }
                    catch (Exception sqlEx)
                    {
                        // En phase de dev, on garde le détail, en prod on pourra le simplifier
                        return StatusCode(500, new
                        {
                            error = "Erreur d'exécution PostgreSQL",
                            details = sqlEx.Message,
                            query = cleanedSql
                        });
                    }
                }

                // Not a SQL query -> return the model text
                return Ok(new { answer = text });
            }
            catch (HttpRequestException hrex)
            {
                // Return a 502 for upstream HTTP errors (Mistral)
                return StatusCode(502, new { error = "Mistral API error", details = hrex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

}
