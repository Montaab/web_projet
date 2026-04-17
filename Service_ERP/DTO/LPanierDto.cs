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

    public virtual Article IdArtNavigation { get; set; }

    public virtual PanierDto IdPanNavigation { get; set; }
    
}
