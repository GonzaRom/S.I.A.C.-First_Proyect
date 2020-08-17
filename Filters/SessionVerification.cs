using System;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Controllers;
using S.I.A.C.Models.DomainModels;

namespace S.I.A.C.Filters
{
    public class SessionVerification : ActionFilterAttribute
    {
        private people objPeople;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);

                objPeople = (people) HttpContext.Current.Session["User"];
                if (objPeople == null)
                    if (filterContext.Controller is LoginController == false)
                        filterContext.HttpContext.Response.Redirect("/Login/Login");
            }
            catch (Exception e)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
            }
        }
    }
}