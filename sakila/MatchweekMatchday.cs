using System;
using System.Collections.Generic;

namespace MatchTechWebsite.sakila;

public partial class MatchweekMatchday
{
    public int Id { get; set; }
    public int MatchWeekKey { get; set; }

    public int MatchDayKey { get; set; }

    public virtual Matchdayfixture MatchDayKeyNavigation { get; set; } = null!;

    public virtual Matchweekfixture MatchWeekKeyNavigation { get; set; } = null!;
}
