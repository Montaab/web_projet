using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class CommandeDto
{
    public int IdCom { get; set; }

    public DateOnly? DateCom { get; set; }

    public string Statut { get; set; }

    public decimal? Total { get; set; }

    public string ModePaiement { get; set; }

    public int? IdClt { get; set; }

    public virtual ICollection<FactureDto> Factures { get; set; } = new List<FactureDto>();

    public virtual ClientDto IdCltNavigation { get; set; }

    public virtual ICollection<LCommandeDto> LCommandes { get; set; } = new List<LCommandeDto>();
    
}
