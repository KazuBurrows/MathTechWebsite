using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTechWebsite.Models
{
	public class MatchWeek
	{

		public int Id { get; }
		public int Week { get; }
		public List<MatchDay> MatchDays = new List<MatchDay>();

		public MatchWeek(int id, int week)
		{
			Id = id;
			Week = week;
		}

		public void Add(MatchDay m)
		{
			MatchDays.Add(m);
		}

		public void Remove(MatchDay m)
		{
			MatchDays.Remove(m);
		}

		public override string ToString()
		{
			if (MatchDays.Count() == 0)
			{
				return "";
			}

			string str = "";
			foreach (MatchDay m in MatchDays)
			{
				try
				{
					str += m.Match.HomeTeamKeyNavigation.Name + " vs " + m.Match.AwayTeamKeyNavigation.Name + " ";
				}
				catch {
					str += " null ";
				}
				
			}

			return str;
		}
	}
}
