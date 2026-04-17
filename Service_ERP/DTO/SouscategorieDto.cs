using Core.Entities;
﻿using System;
using System.Collections.Generic;

namespace Service_ERP.DTO;

public partial class SouscategorieDto
{
    public int IdScat { get; set; }

    public string CodeScat { get; set; }

    public string Libelle { get; set; }

    public string Description { get; set; }

    public int? IdCat { get; set; }

    public DateTime? DateCreation { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual CategorieDto IdCatNavigation { get; set; }
    
}
