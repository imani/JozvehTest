using System.Web.Mvc;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet
{
    public class BookletAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Booklet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Booklet_default",
                "Booklet/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
