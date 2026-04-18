using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class LFournisseurDto
{
    public int IdFour { get; set; }

    public int IdArt { get; set; }

    public int? DelaiLivraison { get; set; }

    public decimal? PrixFournisseur { get; set; }

    public virtual Article IdArtNavigation { get; set; }

    public virtual FournisseurDto IdFourNavigation { get; set; }
    
}
