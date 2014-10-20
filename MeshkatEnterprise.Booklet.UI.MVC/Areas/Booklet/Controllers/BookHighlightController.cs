using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class BookHighlightController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddHighlight(List<BookHighlight> hl)
        {
            return
                Json(
                    ComponentContextBinder.Lookup().GetComponent<IBookHighlightService>().AddHighlight(hl),
                    JsonRequestBehavior.AllowGet);
        }
    }
}