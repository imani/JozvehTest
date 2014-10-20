using System;
using System.Collections.Generic;


namespace MeshkatEnterprise.Booklet.Entity
{
    public class DocInformation
    {
        public String Type { get; set; }
        public long ParagraphId { get; set; }
        public String BookName { get; set; }
        public String Text { get; set; }
        public BookVolume Volume { get; set; }
        public int Page { get; set; }
        public String TocId { get; set; }
    }
}