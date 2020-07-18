using System.Web.Mvc;

namespace S.I.A.C.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult UnauthorizedOperation(string operation, string module, string msjeErrorExcepcion)
        {
            ViewBag.operation = operation;
            ViewBag.module = module;
            ViewBag.msjeErrorExcepcion = msjeErrorExcepcion;
            return View();
        }
    }
}