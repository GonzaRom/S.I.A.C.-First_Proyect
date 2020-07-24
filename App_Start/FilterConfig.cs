using S.I.A.C.Filters;
using System.Web.Mvc;

namespace S.I.A.C
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionVerification());
        }
    }
}
