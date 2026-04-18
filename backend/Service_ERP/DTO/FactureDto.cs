using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class FactureDto
{
    public int IdFact { get; set; }

    public DateOnly? DateFact { get; set; }

    public decimal? MontantTotal { get; set; }

    public string ModePaiement { get; set; }

    public int? IdCom { get; set; }

    public virtual CommandeDto IdComNavigation { get; set; }

    
}
