using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Facture
{
    public int IdFact { get; set; }

    public DateOnly? DateFact { get; set; }

    public decimal? MontantTotal { get; set; }

    public string ModePaiement { get; set; }

    public int? IdCom { get; set; }

    public virtual Commande IdComNavigation { get; set; }
}
