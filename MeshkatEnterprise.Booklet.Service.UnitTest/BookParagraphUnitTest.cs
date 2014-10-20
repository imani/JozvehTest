using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class BookParagraphUnitTest
    {
        public BookParagraphUnitTest()
        {
            ContextUtil.Init();
            LoginUtil.Login();
        }

        [TestMethod]
        public void GetParagraphsBlock()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var paragraphService = context.GetComponent<IBookParagraphService>();
            long startParagraph = 228984;
            long endParagraph = 228985;
            long volumeId = 3;
            TServiceResult<BookParagraphsBlock> result = paragraphService.GetParagraphsBlock(startParagraph, endParagraph, volumeId);
            Assert.IsTrue(result.Successful && result.ReturnValue != null);
            List<BookParagraph> paragraphs = result.ReturnValue.Paragraphs;
            Assert.IsTrue(
                paragraphs[0].ParagraphId == startParagraph && paragraphs[paragraphs.Count-1].ParagraphId == endParagraph
                && paragraphs[0].ParagraphPageNumber == 7
                && paragraphs[0].ParagraphText.Equals("الجزء الأول‌")
                && paragraphs[0].VolumeInfo != null && paragraphs[0].TableOfContentNode != null
                && paragraphs[0].VolumeInfo.Book.BookId == 22 && paragraphs[0].VolumeInfo.VolumeNumber == 1
                && paragraphs[0].TableOfContentNode.Key == 458486
                && paragraphs[0].TableOfContentNode.BookParagraphId == paragraphs[0].ParagraphId
                && paragraphs[0].TableOfContentNode.VolumeId == paragraphs[0].VolumeInfo.VolumeId
                , result.Response != null ? result.Response.Message : "");
        }

        [TestMethod]
        public void GetParagraphsByPageNumber()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var paragraphService = context.GetComponent<IBookParagraphService>();
            long volumeId = 3;
            int pageNumber = 7;
            int fetchSizeBefore = 1;
            int fetchSizeAfter = 1;
            TServiceResult<BookParagraphsBlock> result = paragraphService.GetParagraphsByPageNumber(volumeId, pageNumber,
                fetchSizeBefore, fetchSizeAfter);
            Assert.IsTrue(result.Successful && result.ReturnValue != null && result.ReturnValue.Paragraphs != null);
            List<BookParagraph> paragraphs = result.ReturnValue.Paragraphs;
            Assert.IsTrue(
                paragraphs[1].ParagraphId == 228984 &&
                paragraphs[1].ParagraphPageNumber == pageNumber
                && paragraphs[1].VolumeInfo.VolumeId == volumeId && paragraphs[1].VolumeInfo.Book.BookId == 22
                && paragraphs[1].ParagraphText.Contains("الجزء الأول")
                && paragraphs[1].TableOfContentNode != null
                && paragraphs[1].TableOfContentNode.Key == 458486
                , result.Response != null ? result.Response.Message : "");
        }
    }
}