using AutoMapper;
using Core.Entities;
namespace Service_ERP.DTO;

public partial class ArticleDto
{
    public int IdArt { get; set; }

    public string Designation { get; set; }

    public string Description { get; set; }

    public decimal? PrixUnitaire { get; set; }

    public int? StockDispo { get; set; }

    public DateTime? DateAjout { get; set; }

    public string ImageUrl { get; set; }

    public int? IdScat { get; set; }

    public virtual SouscategorieDto IdScatNavigation { get; set; }

    public virtual ICollection<LCommandeDto> LCommandes { get; set; } = new List<LCommandeDto>();

    public virtual ICollection<LFournisseurDto> LFournisseurs { get; set; } = new List<LFournisseurDto>();

    public virtual ICollection<LPanierDto> LPaniers { get; set; } = new List<LPanierDto>();

    
}
