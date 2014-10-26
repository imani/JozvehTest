using System;
using Compositional.Composer;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class BookCommentTypeUnitTest
    {
        public BookCommentTypeUnitTest()
        {
            ContextUtil.Init();
        }
        [TestMethod]
        public void GetCommentTypesUnitTest()
        {
            long bookId = 24;
            ComponentContext context = ComponentContextBinder.Lookup();
            var commentTypeService = context.GetComponent<IBookCommentTypeService>();
            var result = commentTypeService.GetCommentTypes(bookId);
            Assert.IsTrue(result.Successful && result.ReturnValue != null && result.ReturnValue.Count == 8);

        }
    }
}
