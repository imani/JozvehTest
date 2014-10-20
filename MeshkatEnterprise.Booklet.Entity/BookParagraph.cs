using System;
using System.Collections.Generic;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class BookParagraph
    {
        public BookVolume VolumeInfo { get; set; }
        public long ParagraphId { get; set; }
        public string ParagraphText { get; set; }
        public BookTableOfContent TableOfContentNode { get; set; }
        public int ParagraphPageNumber { get; set; }
        public List<Footnote> Footnotes { get; set; }
    }
}
