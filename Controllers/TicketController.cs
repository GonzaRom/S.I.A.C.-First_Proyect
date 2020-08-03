using System.Web.Mvc;
using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;
using S.I.A.C.Service;
using S.I.A.C.Service.Implement;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketCommandsService _ticketCommandsService;
        private readonly TicketQueriesService _ticketQueriesService;
        private readonly ViewUtilityServices _viewUtilityServices;
        private readonly SearchQueriesService _searchQueriesService;

        public TicketController()
        {
            _ticketQueriesService = new TicketQueriesService();
            _viewUtilityServices = new ViewUtilityServices();
            _ticketCommandsService = new TicketCommandsService();
            _searchQueriesService = new SearchQueriesService();
        }

        [HttpGet]
        public ActionResult Ticket()
        {
            ViewBag.priorities = _viewUtilityServices.GetListOfPriorities();
            ViewBag.categories = _viewUtilityServices.GetListOfCategories();
            ViewBag.technician = _viewUtilityServices.GetListOfTechnicians();
            ViewBag.clients = _viewUtilityServices.GetListOfClients();

            var baseTicket = new TicketViewModel();
            var encryptedTicketId = Encrypt.GetSHA256(baseTicket.internalId.ToString());
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(baseTicket);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(9)]
        public ActionResult Ticket(TicketViewModel baseTicket, string internalId)
        {
            if (internalId != Encrypt.GetSHA256(baseTicket.internalId.ToString()) || !ModelState.IsValid)
                return View(baseTicket);

            var (result, idLocal) = _ticketCommandsService.CreateTicket(baseTicket, (people) Session["User"]);

            if (!result)
            {
                ViewBag.Error = "E R R O R al crear el ticket";
                return View(baseTicket);
            }

            TempData["Successful"] = "Ticket creado Exitosamente!";
            TempData["TicketNumber"] = "Ticket Numero: " + idLocal;
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult CurrentTickets()
        {
            var currentPeople = (people) Session["User"];
            var activeTickets = _ticketQueriesService.GetTicketsList(currentPeople);

            return View(activeTickets);
        }

        [HttpGet]
        [HandleError]
        [AuthorizeUser(11)]
        public ActionResult Edit(string internalId)
        {
            var ticketId = Encrypt.Unprotect(internalId);
            var ticket = _ticketQueriesService.GeTicketViewModel(int.Parse(ticketId));

            if (ticket == null) return RedirectToAction("UnauthorizedOperation", "Error");

            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            //encrypt the ticket id to the webpage//
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(11)]
        public ActionResult Edit(string internalId, TicketViewModel ticketViewModel)
        {
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var ticketId = Encrypt.Unprotect(internalId);
            var result = _ticketCommandsService.EditTicket(ticketViewModel, ticketId);
            if (result == false)
            {
                var encryptedTicketId = Encrypt.Protect(ticketId);
                ViewBag.TicketIdEncrypt = encryptedTicketId;
                ViewBag.Error = "E R R O R al editar el ticket";
                return View(ticketViewModel);
            }

            TempData["Successful"] = "Ticket editado correctamente!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AuthorizeUser(11)]
        public ActionResult Detail(string internalId)
        {
            var toSearch = (SearchViewModel) Session["toSearch"];
            if (toSearch == null) return RedirectToAction("UnauthorizedOperation", "Error");
            if (internalId == null) internalId = toSearch.toSearch;

            var ticketId = Encrypt.Unprotect(internalId);
            var currentTicket = _searchQueriesService.SearchTicketByNumber(int.Parse(ticketId));

            if (currentTicket == null) return RedirectToAction("UnauthorizedOperation", "Error");

            var ticketHistory = _ticketQueriesService.GeTicketHistoryViewModel(int.Parse(ticketId));
            ViewBag.CurrentTicket = currentTicket;
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticketHistory);
        }

        [HttpGet]
        [AuthorizeUser(11)]
        public ActionResult Update(string internalId)
        {
            var ticketId = Encrypt.Unprotect(internalId);
            var ticketHistoryView = new TicketHistoryViewModel();
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticketHistoryView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(string internalId, TicketHistoryViewModel ticketHistoryViewModel)
        {
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var ticketId = Encrypt.Unprotect(internalId);
            var currentPeople = (people) Session["User"];
            ticketHistoryViewModel.idPeople = currentPeople.id;

            var result = _ticketCommandsService.UpdateTicket(ticketHistoryViewModel, ticketId);
            if (result == false)
            {
                var encryptedTicketId = Encrypt.Protect(ticketId);
                ViewBag.TicketIdEncrypt = encryptedTicketId;
                ViewBag.Error = "E R R O R al actualizando el ticket";
                return View(ticketHistoryViewModel);
            }

            TempData["Successful"] = "Ticket editado correctamente!";
            return RedirectToAction("Index", "Home");
        }
    }
}