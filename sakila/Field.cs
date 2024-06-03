using System;
using System.Collections.Generic;

namespace MatchTechWebsite.sakila;

public partial class Field
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Type { get; set; } = null!;


    public virtual ICollection<Matchdayfixture> Matchdayfixtures { get; set; } = new List<Matchdayfixture>();
}
