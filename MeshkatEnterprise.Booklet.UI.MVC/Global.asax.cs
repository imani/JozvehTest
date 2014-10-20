using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compositional.Composer;
using Compositional.Composer.Utility;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Infrastructure.Web;
using MeshkatEnterprise.Infrastructure.Web.Security;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.UI.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ResourceGlobalizer.Init(Server.MapPath("~/App_GlobalResources"));

            var context = new ComponentContext();
#if DEBUG
            context.ProcessCompositionXmlFromResource("MeshkatEnterprise.Booklet.UI.MVC.Composition-Debug.xml");
#else
                context.ProcessCompositionXmlFromResource("MeshkatEnterprise.Booklet.UI.MVC.Composition-Release.xml");
#endif
            ComponentContextBinder.Bind(context);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            SecurityToken token = SecurityTokenBinder.Lookup();
            if (token != null)
            {
                SecurityHelper.AddCookie(Response, token);
            }
        }
    }
}