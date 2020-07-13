using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Models;

namespace S.I.A.C.Filters
{
    ///Filtro para verificar si tiene permiso el usuario
    /// Verifica por metodo y no permite multiples
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private people objPeople;
        private dbSIACEntities database = new dbSIACEntities();
        private int idOperation;

        public AuthorizeUser(int idOperation = 0)
        {
            this.idOperation = idOperation;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var nameOperation = "";
            var nameModule = "";
            try
            {
                objPeople = (people) HttpContext.Current.Session["User"];
                var userOperations =
                    database.rolOperations.Where(m => m.idRol == objPeople.idRol && m.idOperations == idOperation);

                if (userOperations.ToList().Count < 1)
                {
                    var operation = database.operations.Find(idOperation);
                    int idModule = operation.idModule;
                    nameOperation = getOperationName(idOperation);
                    nameModule = getModuleName(idModule);
                    filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operation=" + nameOperation + "&modulo=" + nameModule + "&msjeErrorExcepcion=");
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operation=" + nameOperation + "&modulo=" + nameModule + "&msjeErrorExcepcion=");
            }
        }
        public string getOperationName(int idOperation)
        {
            var ope = from op in database.operations
                where op.id == idOperation
                select op.name;
            string nameOperation;
            try
            {
                nameOperation = ope.First();
            }
            catch (Exception)
            {
                nameOperation = "";
            }
            return nameOperation;
        }

        public string getModuleName(int? idModule)
        {
            var modulo = from m in database.module
                where m.id == idModule
                select m.name;

            string nameModule;
            try
            {
                nameModule = modulo.First();
            }
            catch (Exception)
            {
                nameModule = "";
            }
            return nameModule;
        }
    }
}