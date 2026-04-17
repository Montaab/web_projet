using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Fournisseur
{
    public int IdFour { get; set; }

    public string NomSociete { get; set; }

    public string Tel { get; set; }

    public string Email { get; set; }

    public string Adresse { get; set; }

    public string Ville { get; set; }

    public virtual ICollection<LFournisseur> LFournisseurs { get; set; } = new List<LFournisseur>();
}
