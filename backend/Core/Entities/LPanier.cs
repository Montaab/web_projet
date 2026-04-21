using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class LPanier
{
    public int IdPan { get; set; }

    public int IdArt { get; set; }

    public int? Quantite { get; set; }

    public DateTime? DateAjout { get; set; }

    public virtual Article IdArtNavigation { get; set; }

    public virtual Panier IdPanNavigation { get; set; }
}
