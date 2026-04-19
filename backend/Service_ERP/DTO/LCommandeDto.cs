using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class LCommandeDto
{
    public int IdCom { get; set; }

    public int IdArt { get; set; }

    public int? Quantite { get; set; }

    public decimal? PrixAchat { get; set; }

    public decimal? Remise { get; set; }

    public virtual ArticleDto? IdArtNavigation { get; set; }

    public virtual CommandeDto? IdComNavigation { get; set; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<LCommande, LCommandeDto>().ReverseMap();
            profile.CreateMap<Article, ArticleDto>().ReverseMap();
            profile.CreateMap<Commande, CommandeDto>().ReverseMap();
    }

}
