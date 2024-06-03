using MatchTechWebsite.Models;
using MatchTechWebsite.sakila;
using MatchTechWebsite.ViewComponents;

namespace MatchTechWebsite.Models
{
    public class ManagerViewModel
    {
        //public List<Matchdayfixture>? Matchdayfixtures { get; set; }

        public List<sakila.Tournament>? Tournaments { get; set; }
    }


    public class ManagerCreateViewModel
    {
        public List<string> TournamentSelector = new List<string>() { "Round Robin", "Knockout" };
        public List<sakila.Team> TeamSelector { get; set; }

        public string TournamentName { get; set; }
        public string TournamentType { get; set; }
        public List<int> Teams { get; set; }
        public DateTime StartDate { get; set; }
        public List<DateTime> HolidayDates { get; set; }
        public List<DateTime> SpecialRequests { get; set; }
        public List<string> MatchDays { get; set; }
        public int Rounds { get; set; }

    }


    public class ManagerEditViewModel
    {
        public sakila.Tournament? tournament { get; set; }

        //public List<MatchweekMatchday>? MatchweekMatchdays { get; set; }
        public List<MatchWeek>? MatchWeeks { get; set; }

        //public Matchdayfixture Matchdayfixture { get; set; }

        //public MatchTile MatchTile { get; set; }
    }


    public class ManagerDeleteViewModel
    {
        public sakila.Tournament? tournament { get; set; }

    }


}
