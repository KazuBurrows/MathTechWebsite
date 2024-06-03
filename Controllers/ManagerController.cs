using MatchTechWebsite.Models;
using MatchTechWebsite.sakila;
using MatchTechWebsite.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace MatchTechWebsite.Controllers
{
    public class ApiParam {
        public string TournamentName { get; set; }
        public string TournamentType { get; set; }
        public List<int> Teams { get; set; }
        public DateTime StartDate { get; set; }
        public int Rounds { get; set; }
    }

    public class ManagerController : Controller
    {
        DbAa1c85MtechContext _context = new DbAa1c85MtechContext();
        public IActionResult Index()
        {
            var viewModel = new ManagerViewModel
            {
                Tournaments = _context.Tournaments.ToList()
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {


            var viewModel = new ManagerCreateViewModel
            {
                TeamSelector = _context.Teams.ToList()

            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string TournamentName, string TournamentType, List<int> Teams, DateTime StartDate, int Rounds)
        {
            Console.WriteLine(TournamentName);
            Console.WriteLine(TournamentType);
            Console.WriteLine(Teams.Count());
            foreach (var team in Teams)
            {
                Console.WriteLine(team);
            }
            Console.WriteLine(StartDate);
            Console.WriteLine(Rounds);

            string EmpInfo  = "none";
            string Baseurl = "http://localhost:5000/";
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                var jsonParam = new ApiParam { TournamentName = TournamentName, TournamentType = TournamentType, Teams = Teams, StartDate = StartDate, Rounds = Rounds };

                HttpResponseMessage Res = await client.PostAsJsonAsync("api/TournamentController", jsonParam);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(EmpResponse);

                    //Deserializing the response recieved from web api and storing into the Employee list
                    //EmpInfo = JsonConvert.DeserializeObject<string>(EmpResponse);
                }

                //Console.WriteLine(EmpInfo);
                //returning the employee list to view
                //return View();
            }




            var viewModel = new ManagerViewModel
            {
                Tournaments = _context.Tournaments.ToList()
            };
            return View(nameof(Index), viewModel);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            sakila.Tournament t = _context.Tournaments.Where(t => t.Id == id).FirstOrDefault();
            List<MatchweekMatchday> matchweek_matchdays = _context.MatchweekMatchdays.Where(m => m.MatchWeekKeyNavigation.TournamentKey == id).ToList();
            List<MatchWeek> matchweeks = new List<MatchWeek> ();
            MatchWeek matchWeek;
            MatchDay matchDay;
            int? matchWeekIndex;
            foreach (var item in matchweek_matchdays)
            {
                matchWeek = new MatchWeek(item.MatchWeekKey, item.MatchWeekKeyNavigation.Week);
                matchDay = new MatchDay(item.MatchDayKeyNavigation);
                
                matchWeekIndex = WeekExistsAtIndex(matchweeks, matchWeek.Week);
                if (matchWeekIndex == null)
                {
                    matchweeks.Add(matchWeek);
                    matchweeks[^1].Add(matchDay);
                }
                else {
                    matchweeks[(int)matchWeekIndex].Add(matchDay);
                }
            
            }

            var viewModel = new ManagerEditViewModel
            {
                tournament = t,
                MatchWeeks = matchweeks
            };

            return View(viewModel);
        }



        [HttpPost]
        public JsonResult Edit(string action, int id, DateTime datetime, int weekId=0, int fieldId=0)
        {
            Console.WriteLine(action);
            Console.WriteLine(id);
            Console.WriteLine(datetime);
            Console.WriteLine(weekId);
            JsonResult jsonReponse;
            switch (action)
            {
                case "ChangeDate":
                    // code block
                    jsonReponse = ChangeDate(id, datetime, fieldId);
                    break;
                case "MoveTile":
                    // code block
                    jsonReponse = MoveTile(id, datetime, weekId);
                    break;
                default:
                    // code block
                    jsonReponse = Json(new { weekId = weekId });
                    break;
            }

            return jsonReponse;
        }


        public JsonResult MoveTile(int id, DateTime datetime, int weekId)
        {
            Console.WriteLine(id);
            Console.WriteLine(datetime);
            Console.WriteLine(weekId);
            var matchWeekMatchDay = _context.MatchweekMatchdays.Where(m => m.MatchDayKey == id).FirstOrDefault();
            var newWeek = _context.Matchweekfixtures.Where(w => w.Id == weekId).FirstOrDefault().Week;
            int currWeek = matchWeekMatchDay.MatchWeekKeyNavigation.Week;

            int dateTimeOffset = newWeek - currWeek;

            var newDateTime = datetime.AddDays(dateTimeOffset * 7);
            matchWeekMatchDay.MatchWeekKey = weekId;
            matchWeekMatchDay.MatchDayKeyNavigation.DateTime = newDateTime;

            _context.SaveChanges();
            return Json(new { weekId = weekId, dateTime = newDateTime });
        }


        public JsonResult ChangeDate(int id, DateTime datetime, int fieldId)
        {
            Console.WriteLine(id);
            Console.WriteLine(datetime);
            Console.WriteLine(fieldId);
            var matchdayfixture = _context.Matchdayfixtures.Where(m => m.Id == id).FirstOrDefault();
            matchdayfixture.DateTime = datetime;
            matchdayfixture.FieldKey = fieldId;

            _context.SaveChanges();
            return Json(new { dateTime = datetime, fieldId = fieldId });
        }


        [HttpGet]
        public JsonResult AvailableFields(int id)
        {
            var matchday = _context.Matchdayfixtures.Find(id);
            var matchdayfixtures = _context.Matchdayfixtures.Where(m => m.DateTime.Date == matchday.DateTime.Date).ToList();
            var allFields = _context.Fields.ToList();
            var usedFields = (from m in matchdayfixtures
                              join f in _context.Fields
                              on m.FieldKey equals f.Id
                              select f).ToList();

            var availableFields = allFields
                   .Where(x => !usedFields.Any(y => y.Id == x.Id));

            Console.WriteLine("AvailableFields");

            foreach (var f in availableFields)
            {
                Console.WriteLine(f.Name);
            }

            return Json(new { fields = availableFields });
        }



        [HttpGet]
        public IActionResult Delete(int? id)
        {

            var tournament = _context.Tournaments.Where(t => t.Id == id).FirstOrDefault();
            return View(tournament);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var matchweekfixtures = _context.Matchweekfixtures.Where(m => m.TournamentKey == id);

            var query = from m in matchweekfixtures
                        join w in _context.MatchweekMatchdays
                        on m.Id equals w.MatchWeekKey
                        select w;

            var matchdayfixtures = from p in query
                                   select p.MatchDayKeyNavigation;

            _context.Matchweekfixtures.RemoveRange(matchweekfixtures);
            _context.Matchdayfixtures.RemoveRange(matchdayfixtures);


            //foreach (var matchweek in matchweekfixtures)
            //{
            //    var m = _context.MatchweekMatchdays.Where(m => m.MatchWeekKey == matchweek.Id).FirstOrDefault();
            //    if (m != null) {
            //        _context.Remove(m);
            //    }

            //}

            var t = _context.Tournaments.Where(t => t.Id == id).FirstOrDefault();
            _context.Remove(t);

            _context.SaveChanges();

            var viewModel = new ManagerViewModel
            {
                Tournaments = _context.Tournaments.ToList()
            };
            return View(nameof(Index), viewModel);
        }


        public IActionResult MatchTileComponent(Matchdayfixture match)
        {
            return ViewComponent("MatchTile", new { match });
        }









        [HttpPost]
        public JsonResult ChangeWeek(int id, int week=4)
        {
            Console.WriteLine("ChangeWeek");
            Console.WriteLine(id);
            Console.WriteLine(week);
            MatchweekMatchday? matchweekMatchday = _context.MatchweekMatchdays.Where(t => t.MatchDayKey == id).FirstOrDefault();
            Matchweekfixture? matchweekfixture = _context.Matchweekfixtures.Where(t => t.Week == week).FirstOrDefault();

            Console.WriteLine(matchweekMatchday.MatchWeekKey);
            Console.WriteLine(matchweekfixture.Id);
            Console.WriteLine(matchweekfixture.Week);

            matchweekMatchday.MatchWeekKey = matchweekfixture.Id;
            _context.SaveChanges();
            return Json("");
        }


        //[HttpGet]
        //public JsonResult AvailableFields(int id)
        //{
        //    Console.WriteLine("AvailableFields");
        //    MatchweekMatchday match = _context.MatchweekMatchdays.Where(m => m.MatchDayKey == id).FirstOrDefault();

        //    var query = (from f in _context.Fields
        //                         join m in _context.MatchweekMatchdays.Where(d => d.MatchWeekKey == match.MatchWeekKeyNavigation.Week)
        //                            on f.Id equals m.MatchDayKeyNavigation.FieldKey into grouping
        //                         from m in grouping.DefaultIfEmpty()
        //                         select new { f, m });



        //    List<Field> fields = new List<Field>();
        //    Field field;

        //    field = new Field();
        //    field.Id = match.MatchDayKeyNavigation.FieldKeyNavigation.Id;
        //    field.Name = match.MatchDayKeyNavigation.FieldKeyNavigation.Name;
        //    fields.Add(field);

        //    foreach (var item in query)
        //    {
        //        if (item.m == null) {
        //            field = new Field();
        //            field.Id = item.f.Id;
        //            field.Name = item.f.Name;
        //            fields.Add(field);
        //        }
                

        //    }
        //    return Json(fields);
        //}


        //private bool IsConfilictingDateTime(DateTime match, DateTime available)
        //{
        //    TimeSpan timeSpan = match.Subtract(available);
        //    int MinuteDiff = timeSpan.Minutes;
        //    if (MinuteDiff < 100 || )
        //    {
            
        //    }


        //    return false;
        //}

        private int? WeekExistsAtIndex(List<MatchWeek> matchweeks, int week)
        {
            for (int i=0; i<matchweeks.Count(); i++)
            {
                if (matchweeks[i].Week == week)
                {
                    return i;
                }
            }

            return null;
        }

    }
}
