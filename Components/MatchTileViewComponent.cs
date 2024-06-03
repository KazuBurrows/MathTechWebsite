using MatchTechWebsite.Models;
using MatchTechWebsite.sakila;
using Microsoft.AspNetCore.Mvc;

namespace MatchTechWebsite.ViewComponents
{
    public class MatchTileViewComponent : ViewComponent
    {
        MatchDay myMatch;

        public MatchTileViewComponent()
        {
            
        }



        public async Task<IViewComponentResult> InvokeAsync(MatchDay match)
        {
            myMatch = match;
            return await Task.FromResult((IViewComponentResult)View("MatchTile", myMatch));
        }


    }
}
