using System;
using System.Collections.Generic;

namespace MatchTechWebsite.sakila;

public partial class Team
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Club Club { get; set; } = null!;

    public virtual ICollection<Matchdayfixture> MatchdayfixtureAwayTeamKeyNavigations { get; set; } = new List<Matchdayfixture>();

    public virtual ICollection<Matchdayfixture> MatchdayfixtureHomeTeamKeyNavigations { get; set; } = new List<Matchdayfixture>();
}
