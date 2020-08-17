using System.Web.Mvc;

namespace S.I.A.C.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        ///     Get Error Controller.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="module"></param>
        /// <param name="msjeErrorExcepcion"></param>
        /// <returns></returns>
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