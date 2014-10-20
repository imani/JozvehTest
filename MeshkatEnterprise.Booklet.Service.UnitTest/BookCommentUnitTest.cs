using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class BookCommentUnitTest
    {
        public BookCommentUnitTest()
        {
            ContextUtil.Init();
            LoginUtil.Login();
        }

        [TestMethod]
        public long AddCommentUnitTest()
        {
            //Section commentSection = new Section();
            long paragraphId = 228987;
            Section commentSection = new Section();
            commentSection.ParagraphId = paragraphId;
            commentSection.StartOffset = 15;
            commentSection.EndOffset = 25;
            BookComment comment = new BookComment();
            comment.Sections = new List<Section>();
            comment.Sections.Add(commentSection);
            comment.Text = "یک توضیح آزمایشی";
            comment.Type = new BookCommentType();
            comment.Type.BookCommentTypeId = 1;
            ComponentContext context = ComponentContextBinder.Lookup();
            var commentService = context.GetComponent<IBookCommentService>();
            var result = commentService.AddComment(comment);
            Assert.IsTrue(result.Successful == true && result.ReturnValue > 0);
            return result.ReturnValue;
        }

        [TestMethod]
        public void EditCommentUnitTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var commentService = context.GetComponent<IBookCommentService>();
            var commentId = AddCommentUnitTest();
            BookComment comment = new BookComment();
            comment.Text = "یک توضیح آزمایشی ویرایش شده";
            comment.Type = new BookCommentType();
            comment.Type.BookCommentTypeId = 1;
            comment.Id = commentId;
            var result = commentService.EditComment(comment);
            Assert.IsTrue(result.Successful);
        }

        [TestMethod]
        public void RemoveCommentUnitTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var commentService = context.GetComponent<IBookCommentService>();
            long commentId = 279532;
            var result = commentService.RemoveComment(commentId);
            Assert.IsTrue(result.Successful);
        }
    }
}
