using System;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class SearchUnitTest
    {
        public SearchUnitTest()
        {
            ContextUtil.Init();
        }
        [TestMethod]
        public void SearchTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var searchService = context.GetComponent<ISearchService>();
            var query = "اسلام";
            TServiceResult<SearchResultItems> result = searchService.Search(query, 50, 60);
            Assert.IsTrue(result.Successful == true && result.ReturnValue != null);


        }
    }
}
