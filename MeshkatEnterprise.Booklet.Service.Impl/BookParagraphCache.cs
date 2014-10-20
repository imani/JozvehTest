using System.Collections.Generic;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    internal class BookParagraphCache
    {
        public Dictionary<long, BookParagraph> AllParagraphs { set; get; }

        public List<BookSectionStyle> AllStyles { set; get; }
    }
}