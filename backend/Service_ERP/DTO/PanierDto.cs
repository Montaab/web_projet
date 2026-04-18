using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class PanierDto
{
    public int IdPan { get; set; }

    public DateTime? DateCreation { get; set; }

    public int? IdClt { get; set; }

    public virtual ICollection<LPanierDto> LPaniers { get; set; } = new List<LPanierDto>();
    
}
