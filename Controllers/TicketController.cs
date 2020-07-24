using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Service;
using System;
using System.Web.Mvc;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {
        private readonly dbSIACEntities _database;
        private readonly TicketQueriesService _ticketQueriesService;
        private readonly ViewUtilityServices _viewUtilityServices;
        private readonly TicketCommandsService _ticketCommandsService;

        public TicketController()
        {
            _ticketQueriesService = new TicketQueriesService();
            _viewUtilityServices = new ViewUtilityServices();
            _database = new dbSIACEntities();
            _ticketCommandsService = new TicketCommandsService();
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
        public ActionResult Ticket(TicketViewModel baseTicket, string internalId)
        {
            if ((internalId != Encrypt.GetSHA256(baseTicket.internalId.ToString())) || (!ModelState.IsValid))
            {
                return View(baseTicket);
            }

            var msg = _ticketCommandsService.CreateTicket(baseTicket, (people)Session["User"]);
            if (msg == null)
            {
                ViewBag.Error = msg;
                return View(baseTicket);
            }

            TempData["Successful"] = msg;
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult CurrentTickets()
        {
            var activeTickets = _ticketQueriesService.GetTicketsList();

            return View(activeTickets);
        }

        [HttpGet]
        [HandleError]
        [AuthorizeUser(11)]
        public ActionResult Edit(string internalId)
        {
            var ticketId = Encrypt.Unprotect(internalId);
            var ticket = _ticketQueriesService.GeTicketViewModel(Int32.Parse(ticketId));

            if (ticket == null)
            {
                return RedirectToAction("UnauthorizedOperation", "Error");
            }

            ViewBag.priorities = _viewUtilityServices.GetListOfPriorities();
            ViewBag.categories = _viewUtilityServices.GetListOfCategories();
            ViewBag.technician = _viewUtilityServices.GetListOfTechnicians();
            //encrypt the ticket id to the webpage//
            var encryptedTicketId = Encrypt.GetSHA256(ticketId.ToString());
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(11)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}