using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Booklet.Controllers
{
    public class BookCommentController : Controller
    {
        //
        // GET: /Booklet/BookComment/

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RemoveComment(long commentId)
        {

            return Json(ComponentContextBinder.Lookup().GetComponent<IBookCommentService>().RemoveComment(commentId), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult EditComment(BookComment comment)
        {

            return Json(ComponentContextBinder.Lookup().GetComponent<IBookCommentService>().EditComment(comment), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddComment(BookComment comment)
        {

             return Json(ComponentContextBinder.Lookup().GetComponent<IBookCommentService>().AddComment(comment));
        }
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetCommentTypes(long bookId)
        {

            return Json(ComponentContextBinder.Lookup().GetComponent<IBookCommentTypeService>().GetCommentTypes(bookId), JsonRequestBehavior.AllowGet);
        }
    }
}
