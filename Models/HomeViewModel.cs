using MatchTechWebsite.sakila;

namespace MatchTechWebsite.Models
{
    public class HomeViewModel
    {
        
        public List<Tournament> Tournaments { get; set; }

        public int? tournamentId { get; set; }
        public string? SearchString { get; set; }

        public List<Matchdayfixture>? matchdayfixtures { get; set; }
    }
}
