using System;
using System.Collections.Generic;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class BookComment
    {
        public long Id{ get; set; }
        public String Text{ get; set; }
        public long PersonId{ get; set; }
        public String PersonName { get; set; }
        public DateTime CreationDateTime{ get; set; }
        public List<BookCommentSubject> Subjects{ get; set; }
        public BookCommentType Type{ get; set; }
        public List<Section> Sections{ get; set; }
        public List<BookParagraph> RelatedParagraphs{ get; set; }
        public List<BookCommentFieldValue> FieldValues{ get; set; }
    }
}
