using System;
using System.Collections.Generic;

namespace MatchTechWebsite.sakila;

public partial class Tournament
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Matchweekfixture> Matchweekfixtures { get; set; } = new List<Matchweekfixture>();
}
