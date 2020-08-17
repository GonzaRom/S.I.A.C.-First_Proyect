using System.Web.Mvc;
using S.I.A.C.Filters;

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