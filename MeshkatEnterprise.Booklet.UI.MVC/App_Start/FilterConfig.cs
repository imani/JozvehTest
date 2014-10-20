using System.Web.Mvc;
using MeshkatEnterprise.Infrastructure.Web.Security;

namespace MeshkatEnterprise.Booklet.UI.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizationFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}