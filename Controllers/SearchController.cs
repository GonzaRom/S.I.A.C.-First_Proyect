using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using S.I.A.C.Filters;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;
using S.I.A.C.Service;
using S.I.A.C.Service.Implement;

namespace S.I.A.C.Controllers
{
    public class SearchController : Controller
    {
        private readonly dbSIACEntities _database;
        private readonly SearchQueriesService _searchQueries;

        public SearchController()
        {
            _database = new dbSIACEntities();
            _searchQueries = new SearchQueriesService();
        }

        // GET: Search
        public ActionResult Search()
        {
            var searchModel = new SearchViewModel();
            return View(searchModel);
        }

        [HttpPost]
        [AuthorizeUser(11)]
        public ActionResult Search(SearchViewModel searchViewModel)
        {
            var aSearch = searchViewModel.toSearch;

            if (aSearch.IsNullOrWhiteSpace()) return RedirectToAction("Search", "Search");

            return RedirectToAction("Found", "Search", new {toSearch = aSearch});
        }

        [HttpGet]
        public ActionResult Found(string toSearch)
        {
            var foundeTickets = _searchQueries.SearchTicket(toSearch);
            if (foundeTickets == null)
            {
                ViewBag.Error = "E R R O R en la busqueda";
                return RedirectToAction("Search", "Search");
            }

            return View(foundeTickets);
        }

        public ActionResult SearchID()
        {
            var searchModel = new SearchViewModel();
            return View(searchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchID(SearchViewModel searchViewModel)
        {
            var aSearch = searchViewModel.toSearch;
            if (aSearch.IsNullOrWhiteSpace()) return RedirectToAction("Search", "Search");
            var ticketFounded = _searchQueries.SearchTicketByNumber(int.Parse(aSearch));
            if (ticketFounded == null) return RedirectToAction("Search", "Search");
            searchViewModel.toSearch = Encrypt.Protect(ticketFounded.idLocal.ToString());

            Session["toSearch"] = searchViewModel;
            return RedirectToAction("Detail", "Ticket");
        }
    }
}