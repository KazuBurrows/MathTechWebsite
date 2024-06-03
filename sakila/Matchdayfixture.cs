using System;
using System.Collections.Generic;

namespace MatchTechWebsite.sakila;

public partial class Matchdayfixture
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int HomeTeamKey { get; set; }

    public int AwayTeamKey { get; set; }

    public int FieldKey { get; set; }

    public virtual Team AwayTeamKeyNavigation { get; set; } = null!;

    public virtual Field FieldKeyNavigation { get; set; } = null!;

    public virtual Team HomeTeamKeyNavigation { get; set; } = null!;
}
