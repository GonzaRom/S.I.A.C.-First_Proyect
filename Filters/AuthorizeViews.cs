using System.Linq;
using System.Web;
using S.I.A.C.Models.DomainModels;

namespace S.I.A.C.Filters
{
    /// <summary>
    ///     Filter what buttons or list can see a user depending on a rol and operation.    ///
    /// </summary>
    public class AuthorizeViews
    {
        private static dbSIACEntities _database;

        public static bool IsAuthonized(int idOperation = 0)
        {
            _database = new dbSIACEntities();
            using (_database)
            {
                var people = (people) HttpContext.Current.Session["User"];
                if (people == null) return false;
                var userOperations =
                    _database.rolOperations.Where(m => m.idRol == people.idRol && m.idOperations == idOperation);
                if (userOperations.ToList().Count < 1) return false;

                return true;
            }
        }
    }
}