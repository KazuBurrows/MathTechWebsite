using MatchTechWebsite.sakila;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTechWebsite.Models
{
	public class MatchDay
	{
		public Matchdayfixture Match { get; set; }
		public MatchDay(Matchdayfixture match)
		{
			Match = match;

        }

	}
}
