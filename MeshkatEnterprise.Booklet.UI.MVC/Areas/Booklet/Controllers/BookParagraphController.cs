using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class BookParagraphController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetParagraphsBlock(long startParagraphId, long endParagraphId, long volumeId)
        {
            return
                Json(
                    ComponentContextBinder.Lookup().GetComponent<IBookParagraphService>().GetParagraphsBlock(startParagraphId,endParagraphId, volumeId),
                    JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetParagraphsByPageNumber(long volumeId, int pageNumber,int fetchSizeBefore, int fetchSizeAfter)
        {
            return
                Json(
                    ComponentContextBinder.Lookup().GetComponent<IBookParagraphService>().GetParagraphsByPageNumber(volumeId,pageNumber,fetchSizeBefore,fetchSizeAfter),
                    JsonRequestBehavior.AllowGet);
        }
    }
}