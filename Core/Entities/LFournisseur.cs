using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class LFournisseur
{
    public int IdFour { get; set; }

    public int IdArt { get; set; }

    public int? DelaiLivraison { get; set; }

    public decimal? PrixFournisseur { get; set; }

    public virtual Article IdArtNavigation { get; set; }

    public virtual Fournisseur IdFourNavigation { get; set; }
}
