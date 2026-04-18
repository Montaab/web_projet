using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Client
{
    public int IdClt { get; set; }

    public string Nom { get; set; }

    public string Prenom { get; set; }

    public string Adresse { get; set; }

    public string Email { get; set; }

    public string Telephone { get; set; }

    public DateTime? DateInscription { get; set; }

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}
