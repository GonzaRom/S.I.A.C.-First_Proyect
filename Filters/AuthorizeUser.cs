using S.I.A.C.Models.DomainModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S.I.A.C.Filters
{
    /// Filtro para verificar si tiene permiso el usuario
    /// Verifica por metodo y no permite multiples
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private dbSIACEntities _database;
        private readonly int _idOperation;
        private people _people;

        public AuthorizeUser(int idOperation = 0)
        {
            this._idOperation = idOperation;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _database = new dbSIACEntities();
            using (_database)
            {
                _people = (people) HttpContext.Current.Session["User"];
                var userOperations =
                    _database.rolOperations.Where(m => m.idRol == _people.idRol && m.idOperations == _idOperation);

                if (userOperations.ToList().Count < 1)
                {
                    var operation = _database.operations.Find(_idOperation);
                    var idModule = operation.idModule;
                    var nameOperation = getOperationName(_idOperation);
                    var nameModule = getModuleName(idModule);
                    filterContext.Result =
                        new RedirectResult("~/Error/UnauthorizedOperation?operation=" + nameOperation +
                                           nameModule + "&msjeErrorExcepcion=");
                }
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
            var modulo = from module in _database.module
                where module.id == idModule
                select module.name;

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