using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;
using S.I.A.C.Models.ViewModels;

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

            var msg = _ticketCommandsService.CreateTicket(baseTicket, (people) Session["User"]);
            if (msg == null)
            {
                ViewBag.Error = "E R R O R al crear el ticket";
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
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            //encrypt the ticket id to the webpage//
            var encryptedTicketId = Encrypt.Protect(ticketId.ToString());
            ViewBag.TicketIdEncrypt = encryptedTicketId;

            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(11)]
        public ActionResult Edit(string internalId, TicketViewModel ticketViewModel)
        {
            var ticketId = Encrypt.Unprotect(internalId);
            var result = _ticketCommandsService.EditTicket(ticketViewModel, ticketId);
            if (result == false)
            {
                var encryptedTicketId = Encrypt.Protect(ticketId.ToString());
                ViewBag.TicketIdEncrypt = encryptedTicketId;
                ViewBag.Error = "E R R O R al editar el ticket";
                return View(ticketViewModel);
            }
            else
            {
                TempData["Successful"] = "Ticket editado correctamente!";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Update(string internalId, TicketViewModel ticketViewModel)
        {
            var ticketId = Encrypt.Unprotect(internalId);
            var ticketHistory = _ticketQueriesService.GeTicketHistoryViewModel(int.Parse(ticketId));
            
            if (ticketHistory == null)
            {
                return RedirectToAction("UnauthorizedOperation", "Error");
            }

            ViewBag.priorities = _viewUtilityServices.GetListOfPriorities();
            ViewBag.categories = _viewUtilityServices.GetListOfCategories();
            ViewBag.technician = _viewUtilityServices.GetListOfTechnicians();
            ViewBag.status = _viewUtilityServices.GetListOfStatus();
            //encrypt the ticket id to the webpage//
            var encryptedTicketId = Encrypt.Protect(ticketId.ToString());
            
            ViewBag.TicketIdEncrypt = encryptedTicketId;
            ViewBag.CurrentTicket = _ticketQueriesService.GeTicketPrintableViewModel(int.Parse(ticketId));
            return View(ticketHistory);
        }

        
    }
}