using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Models;

namespace S.I.A.C.Filters
{
    /// Filtro para verificar si tiene permiso el usuario
    /// Verifica por metodo y no permite multiples
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private readonly dbSIACEntities _database = new dbSIACEntities();
        private readonly int _idOperation;
        private people objPeople;

        public AuthorizeUser(int idOperation = 0)
        {
            this._idOperation = idOperation;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var nameOperation = "";
            var nameModule = "";
            try
            {
                objPeople = (people) HttpContext.Current.Session["User"];
                var userOperations =
                    _database.rolOperations.Where(m => m.idRol == objPeople.idRol && m.idOperations == _idOperation);

                if (userOperations.ToList().Count < 1)
                {
                    var operation = _database.operations.Find(_idOperation);
                    var idModule = operation.idModule;
                    nameOperation = getOperationName(_idOperation);
                    nameModule = getModuleName(idModule);
                    filterContext.Result =
                        new RedirectResult("~/Error/UnauthorizedOperation?operation=" + nameOperation +
                                           nameModule + "&msjeErrorExcepcion=");
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operation=" + nameOperation +
                                                          "&modulo=" + nameModule + "&msjeErrorExcepcion=");
            }
        }

        public string getOperationName(int idOperation)
        {
            var ope = from op in _database.operations
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
            var modulo = from m in _database.module
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