using System.Web.Mvc;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class BookVolumeController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetVolume(long volumeId)
        {
            return Json(ComponentContextBinder.Lookup().GetComponent<IBookVolumeService>().GetVolume(volumeId),
                JsonRequestBehavior.AllowGet);
        }
    }
}