using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Categorie
{
    public int IdCat { get; set; }

    public string CodeCat { get; set; }

    public string Libelle { get; set; }

    public string Description { get; set; }

    public DateTime? DateCreation { get; set; }

    public virtual ICollection<Souscategorie> Souscategories { get; set; } = new List<Souscategorie>();
}
