using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Article
{
    public int IdArt { get; set; }

    public string Designation { get; set; }

    public string Description { get; set; }

    public decimal? PrixUnitaire { get; set; }

    public int? StockDispo { get; set; }

    public DateTime? DateAjout { get; set; }

    public string ImageUrl { get; set; }

    public int? IdScat { get; set; }

    public virtual Souscategorie IdScatNavigation { get; set; }

    public virtual ICollection<LCommande> LCommandes { get; set; } = new List<LCommande>();

    public virtual ICollection<LFournisseur> LFournisseurs { get; set; } = new List<LFournisseur>();

    public virtual ICollection<LPanier> LPaniers { get; set; } = new List<LPanier>();
}
