using System;
using System.Web.Mvc;
using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Service;

namespace S.I.A.C.Controllers
{
    [HandleError]
    public class PeoplesController : Controller
    {
        private readonly PeopleService _peopleService;
        private readonly ViewUtilityServices _viewUtilityServices;

        /// <summary>
        /// Pseudo injection.
        /// </summary>
        public PeoplesController()
        {
            _peopleService = new PeopleService();
            _viewUtilityServices = new ViewUtilityServices();

        }

        // GET: Peoples
        public ActionResult Index()
        {
            return View();
        }

        // GET: Peoples/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //START CREATE**//
        [AuthorizeUser(1)]
        public ActionResult Create()
        {
            ViewBag.rols = _viewUtilityServices.GetListOfRols();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(1)]
        public ActionResult Create(PeopleViewModel peopleViewModel)
        {
            var isNewPeople = false;
            try
            {
                if (ModelState.IsValid)
                {
                    isNewPeople = _peopleService.CreatePeople(peopleViewModel);
                }

                if (isNewPeople)
                {
                    var msg = "Usuario creado exitosamente";
                    TempData["Successful"] = msg;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException;
                return View(peopleViewModel);
            }

            return View(peopleViewModel);
        }
        //**FINISH CREATE//

        // GET: Peoples/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Peoples/Edit/5
        [HttpPost]
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

        // GET: Peoples/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Peoples/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}