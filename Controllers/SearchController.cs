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
        private readonly SearchQueriesService _searchQueries;

        public SearchController()
        {
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

            if (aSearch.IsNullOrWhiteSpace()) return RedirectToAction("Search", "Search", new SearchViewModel());

            return RedirectToAction("Found", "Search", new {toSearch = aSearch});
        }

        [HttpGet]
        public ActionResult Found(string toSearch)
        {
            var currentUser = (people) Session["User"];
            var foundTickets = _searchQueries.SearchTicket(toSearch, currentUser.idRol, currentUser.id);
            if (foundTickets == null)
            {
                ViewBag.Error = "E R R O R en la busqueda";
                return RedirectToAction("Search", "Search", new SearchViewModel());
            }

            return View(foundTickets);
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
            var aSearch = searchViewModel.toSearch.Trim();

            if (aSearch.IsNullOrWhiteSpace()) return RedirectToAction("SearchID", "Search", new SearchViewModel());
            if (!int.TryParse(aSearch, out int idToSearch))
                return RedirectToAction("Search", "Search", new SearchViewModel());

            var currentUser = (people) Session["User"];
            var ticketFounded = _searchQueries.SearchTicketByIdAndUser(idToSearch, currentUser.idRol, currentUser.id);

            if (ticketFounded == null) return RedirectToAction("SearchID", "Search", new SearchViewModel());
            searchViewModel.toSearch = Encrypt.Protect(ticketFounded.idLocal.ToString());

            Session["toSearch"] = searchViewModel;
            return RedirectToAction("Detail", "Ticket");
        }
    }
}