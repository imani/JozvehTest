using Compositional.Composer;
using Compositional.Composer.Utility;
using MeshkatEnterprise.Infrastructure.General;
using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.SelfHost;

namespace MeshkatEnterprise.Booklet.UI.WebAPI.SelfHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            //test
            //string authInfo = "88102101" + ":" + "0942600428";
            //authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            //ODgxMDIxMDE6MDk0MjYwMDQyOA==

            var context = new ComponentContext();
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MeshkatEnterprise.Booklet.UI.WebAPI.SelfHost.Composition-Debug.xml");
            context.ProcessCompositionXml(stream);
            ComponentContextBinder.Bind(context);

            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            config.Filters.Add(new BasicAuthorizationFilter());

            config.EnableCors(new EnableCorsAttribute("*", "*", "POST,GET"));

            config.Routes.MapHttpRoute(
               "API Default", "api/{controller}/{id}",
               new { id = RouteParameter.Optional });

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
