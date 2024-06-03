using MatchTechWebsite.Models;
using MatchTechWebsite.sakila;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace MatchTechWebsite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        DbAa1c85MtechContext _context = new DbAa1c85MtechContext();

        public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

        [HttpGet]
        public async Task<IActionResult> Index(int tournamentId, string searchString)
        {
            Console.WriteLine("Index");
            Console.WriteLine(tournamentId);
            
            HomeViewModel viewModel;
            IQueryable<Matchdayfixture> matchdayfixtures = from f in _context.Matchdayfixtures
                                            orderby f.DateTime
                                            select f;

            var tournaments = _context.Tournaments;
            var tournament = tournaments.Find(tournamentId);

            if (!(tournament == null))
            {
                IQueryable<MatchweekMatchday> matchweekmatchday = _context.MatchweekMatchdays;
                var matchweekfixtures = _context.Matchweekfixtures.Where(m => m.TournamentKey == tournament.Id);

                matchweekmatchday = (from m in matchweekmatchday
                                        join w in matchweekfixtures
                                        on m.MatchWeekKey equals w.Id
                                        select m);

                matchdayfixtures = matchweekmatchday.Select(m => m.MatchDayKeyNavigation);
                
            }
            
            
            if (String.IsNullOrEmpty(searchString))
            {
                viewModel = new HomeViewModel
                {
                    Tournaments = await tournaments.ToListAsync(),
                    matchdayfixtures = await matchdayfixtures.ToListAsync()
                };

                return View(viewModel);
            }


            IQueryable<Team> teams = _context.Teams;
            teams = !String.IsNullOrEmpty(searchString) ?
                                                        teams.Where(x => x.Name.Contains(searchString)) :
                                                        teams;
            Console.WriteLine("matchdayfixtures");
            foreach (var t in matchdayfixtures)
            {
                Console.WriteLine(t.DateTime);
            }
            //matchdayfixtures = (from t in teams from m in matchdayfixtures
            //                                    where t.Id == m.HomeTeamKey || t.Id == m.AwayTeamKey
            //                                    select m);

            matchdayfixtures = (from a in teams
                                join b in matchdayfixtures on a.Id equals b.HomeTeamKey
                                select b)
                                .Concat(
                                from a in teams
                                join b in matchdayfixtures on a.Id equals b.AwayTeamKey
                                select b
                                ).Distinct().OrderBy(m => m.DateTime);

            Console.WriteLine("teams");
            foreach (var t in matchdayfixtures)
            {
                Console.WriteLine(t.DateTime);
            }

            viewModel = new HomeViewModel
            {
                Tournaments = await _context.Tournaments.ToListAsync(),
                matchdayfixtures = matchdayfixtures.ToList()
            };

            return View(viewModel);
        }



        public async Task<IActionResult> Fixtures()
        {
            return View(await _context.Matchdayfixtures.ToListAsync());
        }











        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
