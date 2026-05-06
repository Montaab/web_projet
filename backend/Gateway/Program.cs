var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.MapGet("/", () => Results.Ok("Gateway is running"));
app.MapReverseProxy();


app.Run();