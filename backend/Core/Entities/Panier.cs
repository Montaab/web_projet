using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Panier
{
    public int IdPan { get; set; }

    public DateTime? DateCreation { get; set; }

    public int? IdClt { get; set; }

    public virtual ICollection<LPanier> LPaniers { get; set; } = new List<LPanier>();
}
