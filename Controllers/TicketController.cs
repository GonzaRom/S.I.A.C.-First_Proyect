using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Service;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {
        private readonly dbSIACEntities _database;
        private readonly TicketService _ticketService = new TicketService();
        private readonly ViewUtilityServices _viewUtilityServices;

        public TicketController()
        {
            _viewUtilityServices = new ViewUtilityServices();
            _database = new dbSIACEntities();
        }

        [HttpGet]
        public ActionResult Ticket()
        {
            ViewBag.priorities = _viewUtilityServices.GetListOfPriorities();
            ViewBag.categories = _viewUtilityServices.GetListOfCategories();
            ViewBag.technician = _viewUtilityServices.GetListOfTechnicians();
            ViewBag.clients = _viewUtilityServices.GetListOfClients();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ticket(TicketViewModel baseTicket)
        {
            if (ModelState.IsValid)
            {
                var currentUser = (people) Session["User"];
                var ticket = new ticket
                {
                    idStatus = baseTicket.idStatus,
                    idCreatorPeople = currentUser.id,
                    creationDate = baseTicket.creationDate,
                    estimatedFinishDate = baseTicket.estimatedFinishDate,
                    idPriority = baseTicket.idPriority,
                    idAssignedTechnician = baseTicket.idAssignedTechnician,
                    idCategory = baseTicket.idCategory,
                    description = baseTicket.description
                };
                try
                {
                    using (_database)
                    {
                        _database.ticket.Add(ticket);
                        _database.SaveChanges();
                    }

                    var msg = "Ticket creado exitosamente";
                    TempData["Successful"] = msg;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.InnerException;
                    return View(baseTicket);
                }
            }

            return View(baseTicket);
        }

        [HttpGet]
        public ActionResult CurrentTickets()
        {
            var activeTickets = _ticketService.GetTickets();

            return View(activeTickets);
        }

        [HandleError]
        public ActionResult Edit(int? ticketId)
        {
            var ticket = _ticketService.GeTicketViewModel(ticketId);
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