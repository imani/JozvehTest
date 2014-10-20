using Compositional.Composer;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;
using System.Web.Mvc;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class PersonController : Controller
    {
        [System.Web.Mvc.AllowAnonymous]
        [HttpGet]
        public ActionResult GetPerson()
        {
            return
                Json(
                    ComponentContextBinder.Lookup().GetComponent<IPersonService>().GetPerson(SecurityTokenBinder.Lookup().UserName),
                    JsonRequestBehavior.AllowGet);
        }
    }
}