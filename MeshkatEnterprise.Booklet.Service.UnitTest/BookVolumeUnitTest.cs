using Compositional.Composer;
using Compositional.Composer.Utility;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class BookVolumeUnitTest
    {
        public BookVolumeUnitTest()
        {
            var context = new ComponentContext();
            context.ProcessCompositionXmlFromResource("MeshkatEnterprise.Booklet.Service.UnitTest.Composition.xml");
            ComponentContextBinder.Bind(context);
        }

        [TestMethod]
        public void GetVolume()
        {
            ComponentContext context = ComponentContextBinder.Lookup();

            var roots = new TableOfContentUnitTest().GetRoots();

            var volumeService = context.GetComponent<IBookVolumeService>();
            
            TServiceResult<BookVolume> getVolumeResult = volumeService.GetVolume(roots[0].VolumeId);
          
            Assert.IsTrue(getVolumeResult.Successful && getVolumeResult.ReturnValue != null &&
                          getVolumeResult.ReturnValue.StartParagraphId > 0 &&
                          getVolumeResult.ReturnValue.EndParagraphId > 0, getVolumeResult.Response != null ? getVolumeResult.Response.Message : "");
        }
    }
}