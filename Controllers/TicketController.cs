using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Models;

namespace S.I.A.C.Controllers
{
    public class TicketController : Controller
    {

        [HttpGet]
        public ActionResult Ticket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ticket(ticket ticket)
        {

            if (ModelState.IsValid)
            {
                var currentUser = (people) Session["User"];
                var baseTicket = new ticket();
                ticket.status = baseTicket.status;
                ticket.creationDate = baseTicket.creationDate;
                ticket.estimatedFinishDate = baseTicket.estimatedFinishDate;
                ticket.idCreatorPeople = currentUser.id;
               ///remplazar por dropbox
                ticket.idCategory = 1;
                ticket.idPriority = 1;
                try
                {
                    using (Models.dbSIACEntities database = new Models.dbSIACEntities())
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