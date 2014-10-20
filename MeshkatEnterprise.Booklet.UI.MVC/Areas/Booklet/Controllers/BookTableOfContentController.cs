using System.Web.Mvc;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class BookTableOfContentController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetRoots()
        {
            return
                Json(
                    ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetRoots(),
                    JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetChildren(long parentNodeId)
        {
            return
                Json(
                    ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetChildren(parentNodeId),
                    JsonRequestBehavior.AllowGet);
        }

    }
}