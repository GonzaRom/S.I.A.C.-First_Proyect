using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;
using S.I.A.C.Service;
using S.I.A.C.Service.Implement;
using System.Web.Mvc;
using S.I.A.C.Models.MailerModels;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {
        private readonly SearchQueriesService _searchQueriesService;
        private readonly TicketCommandsService _ticketCommandsService;
        private readonly TicketQueriesService _ticketQueriesService;
        private readonly ViewUtilityServices _viewUtilityServices;

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
            var encryptedTicketId = Encrypt.GetSHA256(baseTicket.ticketIdLocal.ToString());
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(baseTicket);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(9)]
        public ActionResult Ticket(TicketViewModel baseTicket, string ticketIdLocal)
        {
            if (!ModelState.IsValid) return View(baseTicket);

            if (ticketIdLocal != Encrypt.GetSHA256(baseTicket.ticketIdLocal.ToString()) || !ModelState.IsValid)
                return View(baseTicket);

            var (result, idLocal) = _ticketCommandsService.CreateTicket(baseTicket, (people) Session["User"]);

            if (!result)
            {
                ViewBag.Error = "E R R O R al crear el ticket";
                return View(baseTicket);
            }

            var currentUser = (people) Session["User"];
            
            var email = new NewTicketEmail
            {
                to = "siac.encargado@gmail.com", //almacenar en constante o tomar datos de lista de encargados
                userName = currentUser.id + currentUser.name + currentUser.lastname,
                comment = baseTicket.description

            };

            email.Send();

            TempData["Successful"] = "Ticket creado Exitosamente!";
            TempData["TicketNumber"] = "Ticket Numero: " + idLocal;
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult CurrentTickets()
        {
            var currentPeople = (people) Session["User"];
            var activeTickets = _ticketQueriesService.GetActiveTicketsList(currentPeople);

            return View(activeTickets);
        }

        [HttpGet]
        [HandleError]
        [AuthorizeUser(11)]
        public ActionResult Edit(string ticketIdLocal)
        {
            var ticketId = Encrypt.Unprotect(ticketIdLocal);
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
        public ActionResult Edit(string ticketIdLocal, TicketViewModel ticketViewModel)
        {
            if (!ModelState.IsValid) return View(ticketViewModel);

            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var ticketId = Encrypt.Unprotect(ticketIdLocal);
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
        public ActionResult Detail()
        {
            var ticketIdLocal=(string) TempData["ticketIdLocal"];
            var toSearch = (SearchViewModel) Session["toSearch"];
            if (toSearch == null && ticketIdLocal == null) return RedirectToAction("UnauthorizedOperation", "Error");
            if (ticketIdLocal == null) ticketIdLocal = toSearch.toSearch;

            var ticketId = Encrypt.Unprotect(ticketIdLocal);
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
        public ActionResult Update(string ticketIdLocal)
        {
            var ticketId = Encrypt.Unprotect(ticketIdLocal);
            var ticketHistoryView = new TicketHistoryViewModel();
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticketHistoryView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(11)]
        public ActionResult Update(string ticketIdLocal, TicketHistoryViewModel ticketHistoryViewModel)
        {
            if (!ModelState.IsValid) return View(ticketHistoryViewModel);

            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var ticketId = Encrypt.Unprotect(ticketIdLocal);
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