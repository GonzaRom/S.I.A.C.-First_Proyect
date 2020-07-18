using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S.I.A.C.Models;
using S.I.A.C.Service;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {
        private readonly ViewUtilityServices viewUtilityServices;
        private readonly dbSIACEntities database;

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
                var ticket = new ticket();
                ticket.creationDate = baseTicket.creationDate;
                ticket.idCreatorPeople = currentUser.id;
                ticket.idAssignedTechnician = baseTicket.idAssignedTechnician;
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
                    return View(ticket);
                }
            }

            return View(ticket);
        }
    }
}