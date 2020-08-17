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
        private readonly MailerService _mailerService;
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
            _mailerService = new MailerService();
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

        /// <summary>
        /// </summary>
        /// <param name="baseTicket"></param>
        /// <param name="ticketIdLocal"></param>
        /// <returns></returns>
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

            _mailerService.NotifyNewTicket((people) Session["User"], baseTicket.description);

            TempData["Successful"] = "Ticket creado Exitosamente!";
            TempData["TicketNumber"] = "Ticket Numero: " + idLocal;
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CurrentTickets()
        {
            var currentPeople = (people) Session["User"];
            var activeTickets = _ticketQueriesService.GetActiveTicketsList(currentPeople);

            return View(activeTickets);
        }

        /// <summary>
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <returns></returns>
        [HttpGet]
        [HandleError]
        [AuthorizeUser(11)]
        public ActionResult Edit(string ticketIdLocal)
        {
            // var ticketIdLocal = (string) TempData["ticketIdLocal"];
            var ticketId = Encrypt.Unprotect(ticketIdLocal);
            var ticket = _ticketQueriesService.GeTicketViewModel(int.Parse(ticketId));

            if (ticket == null) return RedirectToAction("UnauthorizedOperation", "Error");

            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            //encrypt the ticket id to the webpage//
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticket);
        }

        /// <summary>
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <param name="ticketViewModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(11)]
        public ActionResult Detail(string ticketIdLocal)
        {
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

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(11)]
        public ActionResult Update()
        {
            var internalId = (string) TempData["internalId"];
            var ticketId = Encrypt.Unprotect(internalId);
            var ticketHistoryView = new TicketHistoryViewModel();
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticketHistoryView);
        }

        /// <summary>
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <param name="ticketHistoryViewModel"></param>
        /// <returns></returns>
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

            _mailerService.NotifyUpdatedTicket(ticketHistoryViewModel, ticketId);
            TempData["Successful"] = "Ticket editado correctamente!";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <returns></returns>
        [HttpGet]
        [HandleError]
        public ActionResult Delete(string ticketIdLocal)
        {
            //var ticketIdLocal = (string)TempData["ticketIdLocal"];
            var ticketId = Encrypt.Unprotect(ticketIdLocal);
            var currentTicket = _searchQueriesService.SearchTicketByNumber(int.Parse(ticketId));
            var encryptedTicketId = Encrypt.Protect(ticketId);
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(currentTicket);
        }

        /// <summary>
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <param name="ticketPrintableModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(12)]
        public ActionResult Delete(string ticketIdLocal, TicketPrintableModel ticketPrintableModel)
        {
            if (!ModelState.IsValid) return View(ticketPrintableModel);

            var localTicketId = Encrypt.Unprotect(ticketIdLocal);
            if (!int.TryParse(localTicketId, out var ticketId)) return View(ticketPrintableModel);

            var result = _ticketCommandsService.UpdateTicketStatus((int) EStatus.Cancelado,
                _searchQueriesService.SearchTicketId(localTicketId));
            if (result == false)
            {
                var encryptedTicketId = Encrypt.Protect(ticketId.ToString());
                ViewBag.TicketIdEncrypt = encryptedTicketId;
                ViewBag.Error = "E R R O R al actualizando el ticket";
                return View(ticketPrintableModel);
            }

            ticketPrintableModel = _searchQueriesService.SearchTicketByNumber(ticketId);
            var currentPeople = (people) Session["User"];
            _mailerService.NotifyDeletedTicket(ticketPrintableModel, ticketId, currentPeople);
            TempData["Successful"] = "Ticket Eliminado correctamente!";

            return RedirectToAction("Index", "Home");
        }
    }
}