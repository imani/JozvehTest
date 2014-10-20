using System.Web.Mvc;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;
namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Booklet/Search/

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetSearchResult(string query, int start, int end)
        {
            //return Json(ComponentContextBinder.Lookup().GetComponent<ISearchService>().Search(query, start, end),
            //    JsonRequestBehavior.AllowGet);
            return Json(ComponentContextBinder.Lookup().GetComponent<ISearchService>().Search(query, start, end), JsonRequestBehavior.AllowGet);
            

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult TOCTags()
        {

            return Json(ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetNodesTitleAndId(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetPath(long nodeId)
        {

            return Json(ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetPath(nodeId), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetSubTree(long nodeId)
        {

            return Json(ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetSubTree(nodeId), JsonRequestBehavior.AllowGet);
        }
}
}
