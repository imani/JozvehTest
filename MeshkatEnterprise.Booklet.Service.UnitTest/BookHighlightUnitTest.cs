using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class BookHighlightUnitTest
    {
        public BookHighlightUnitTest()
        {
            ContextUtil.Init();
            LoginUtil.Login();
        }
        [TestMethod]
        public void AddHighlightTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            var highlightService = context.GetComponent<IBookHighlightService>();
            var highlights = new List<BookHighlight>();
            var highlight = new BookHighlight();
            var section = new Section();
            section.ParagraphId = 228990;
            section.StartOffset = 0;
            section.EndOffset = 10;
            highlight.HighlightSection = section;
            highlight.PersonId = 4000;
            highlights.Add(highlight);
            var result = highlightService.AddHighlight(highlights);
            Assert.IsTrue(result.Successful && result.ReturnValue != null && result.ReturnValue.Count == highlights.Count);
        }
    }
}
