using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;
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
            return View();
        }

        [HttpGet]
        public ActionResult Found(SearchViewModel searchViewModel)
        {
            

            List<TicketPrintableModel> foundeTickets = _searchQueries.SearchTicket(searchViewModel.toSearch);

            if (foundeTickets==null)
            {
                ViewBag.Error = "E R R O R en la busqueda";
                return RedirectToAction("Search", "Search");
            }

            return View(foundeTickets);

        }
    }
}