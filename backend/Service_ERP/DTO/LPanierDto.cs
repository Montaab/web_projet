using Core.Entities;

using AutoMapper;
using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class LPanierDto
{
    public int IdPan { get; set; }

    public int IdArt { get; set; }

    public int? Quantite { get; set; }

    public DateTime? DateAjout { get; set; }

    public virtual ArticleDto? IdArtNavigation { get; set; }

    public virtual PanierDto? IdPanNavigation { get; set; }

    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<LPanier, LPanierDto>().ReverseMap();
            profile.CreateMap<Article, ArticleDto>().ReverseMap();
            profile.CreateMap<Panier, PanierDto>().ReverseMap();

    }
}