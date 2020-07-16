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
            List<PriorityViewModel> listOfPriorities = null;
            List<CategorisViewModel> listOfCategories = null;
            using (Models.dbSIACEntities database = new Models.dbSIACEntities())
            {
                //listOfPriorities = (database.priority.Select(priorities =>
                //new PriorityViewModel(priorities.id,priorities.name))).ToList(); //<-- Aprendi que en linq a entities el codigo se ejecura
                //en el lado del servidor y los constructores no se pueden trasladar a sql de esta manera

                //Priorities//
                listOfPriorities = (from priority in database.priority select new PriorityViewModel
                        {
                            keyPriority = priority.id,valuePriority = priority.name
                        }
                    ).ToList();

                //Categories//
                listOfCategories = (from cat in database.category select new CategorisViewModel()
                        {
                           keyCategories = cat.id,
                            nameCategories = cat.name
                        }
                    ).ToList();
            }

            List<SelectListItem> prioritiesList = listOfPriorities.ConvertAll(data => new SelectListItem
            {
                Text = data.valuePriority,
                Value = data.keyPriority.ToString(),
                Selected = false
            });

            List<SelectListItem> categoriesList = listOfCategories.ConvertAll(data => new SelectListItem
            {
                Text = data.nameCategories,
                Value = data.keyCategories.ToString(),
                Selected = false
            });

            ViewBag.priorities = prioritiesList;
            ViewBag.categories = categoriesList;

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