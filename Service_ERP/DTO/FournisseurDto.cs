using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class FournisseurDto
{
    public int IdFour { get; set; }

    public string NomSociete { get; set; }

    public string Tel { get; set; }

    public string Email { get; set; }

    public string Adresse { get; set; }

    public string Ville { get; set; }

    public virtual ICollection<LFournisseurDto> LFournisseurs { get; set; } = new List<LFournisseurDto>();
    
}
