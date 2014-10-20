using System.Collections.Generic;
using Compositional.Composer;
using Compositional.Composer.Utility;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class TableOfContentUnitTest
    {
        public TableOfContentUnitTest()
        {
            var context = new ComponentContext();
            context.ProcessCompositionXmlFromResource("MeshkatEnterprise.Booklet.Service.UnitTest.Composition.xml");
            ComponentContextBinder.Bind(context);
        }

        [TestMethod]
        public void GetRootsTest()
        {
            GetRoots();
        }

        [TestMethod]
        public void GetChildrenTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();

            var tableOfContentService = context.GetComponent<IBookTableOfContentService>();

            List<BookTableOfContent> roots = GetRoots();

            TServiceResult<List<BookTableOfContent>> getChildrenResult = tableOfContentService.GetChildren(roots[0].Key);

            Assert.IsTrue(getChildrenResult.Successful,
                getChildrenResult.Response != null ? getChildrenResult.Response.Message : "");
        }

        [TestMethod]
        public void GetSubTreeTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var tableOfContentService = context.GetComponent<IBookTableOfContentService>();

            var result = tableOfContentService.GetSubTree(458496);

            Assert.IsTrue(result.Successful,
                result.Response != null ? result.Response.Message : "");

        }

        [TestMethod]
        public void GetNodesTitleAndIdTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var tableOfContentService = context.GetComponent<IBookTableOfContentService>();
            var result = tableOfContentService.GetNodesTitleAndId();
            Assert.IsTrue(result.Successful,
                result.Response != null ? result.Response.Message : "");
        }

        public List<BookTableOfContent> GetRoots()
        {
            ComponentContext context = ComponentContextBinder.Lookup();

            var tableOfContentService = context.GetComponent<IBookTableOfContentService>();

            TServiceResult<List<BookTableOfContent>> getRootsResult = tableOfContentService.GetRoots();

            Assert.IsTrue(getRootsResult.Successful && getRootsResult.ReturnValue.Count > 0,
                getRootsResult.Response != null ? getRootsResult.Response.Message : "");

            return getRootsResult.ReturnValue;
        }
    }
}