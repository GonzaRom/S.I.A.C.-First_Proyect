﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Models;

namespace S.I.A.C.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDataModel loginDataModel)
        {
            if (ModelState.IsValid)
            {
                return SearchUser(loginDataModel.email, loginDataModel.password);
            }
            else
                return View(loginDataModel);
        }

        public ActionResult SearchUser(string email, string password)
        {
            try
            {
                using (Models.dbSIACEntities database = new Models.dbSIACEntities())
                {
                    var objPeople =
                        database.people.FirstOrDefault(e => e.email == email.Trim() && e.pass == password.Trim());

                    if (objPeople == null)
                    {
                        var msg = "Error, usuario o password incorrecto !";
                        TempData["ErrorMessage"] = msg;
                        return RedirectToAction("Login", "Login");
                    }

                    Session["User"] = objPeople;
                }

                //Acceso Correcto
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Login", "Login");
            }
        }
    }
}