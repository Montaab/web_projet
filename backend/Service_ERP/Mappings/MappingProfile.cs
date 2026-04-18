using AutoMapper;
using Core.Entities;
using Service_ERP.DTO;

namespace Service_ERP.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Categorie, CategorieDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Commande, CommandeDto>().ReverseMap();
            CreateMap<Facture, FactureDto>().ReverseMap();
            CreateMap<Fournisseur, FournisseurDto>().ReverseMap();
            CreateMap<LCommande, LCommandeDto>().ReverseMap();
            CreateMap<LFournisseur, LFournisseurDto>().ReverseMap();
            CreateMap<LPanier, LPanierDto>().ReverseMap();
            CreateMap<Panier, PanierDto>().ReverseMap();
            CreateMap<Souscategorie, SouscategorieDto>().ReverseMap();
        }
    }
}
