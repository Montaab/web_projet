using AutoMapper;
using Core.Entities;
namespace Service_ERP.DTO;

public partial class CategorieDto
{
    public int IdCat { get; set; }

    public string CodeCat { get; set; }

    public string Libelle { get; set; }

    public string Description { get; set; }

    public DateTime? DateCreation { get; set; }

    public virtual ICollection<SouscategorieDto> Souscategories { get; set; } = new List<SouscategorieDto>();
    

}
