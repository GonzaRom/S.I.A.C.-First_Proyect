using System;
using System.Collections.Generic;
using System.Web.Mvc;
using S.I.A.C.Models;
using S.I.A.C.Service;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {
        private readonly dbSIACEntities database;
        private readonly TicketService ticketService = new TicketService();
        private readonly ViewUtilityServices viewUtilityServices;

        public TicketController()
        {
            viewUtilityServices = new ViewUtilityServices();
            database = new dbSIACEntities();
        }

        [HttpGet]
        public ActionResult Ticket()
        {
            ViewBag.priorities = viewUtilityServices.GetListOfPriorities();
            ViewBag.categories = viewUtilityServices.GetListOfCategories();
            ViewBag.technician = viewUtilityServices.GetListOfTechnicians();

            return View();
        }

        [HttpPost]
        public ActionResult Ticket(TicketBindingModel baseTicket)
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
                    using (database)
                    {
                        database.ticket.Add(ticket);
                        database.SaveChanges();
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
            var activeTickets = new List<TicketPrintableModel>();
            activeTickets = ticketService.GetTickets();

            return View(activeTickets);
        }
    }
}