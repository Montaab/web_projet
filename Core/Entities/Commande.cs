using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Commande
{
    public int IdCom { get; set; }

    public DateOnly? DateCom { get; set; }

    public string Statut { get; set; }

    public decimal? Total { get; set; }

    public string ModePaiement { get; set; }

    public int? IdClt { get; set; }

    public virtual ICollection<Facture> Factures { get; set; } = new List<Facture>();

    public virtual Client IdCltNavigation { get; set; }

    public virtual ICollection<LCommande> LCommandes { get; set; } = new List<LCommande>();
}
