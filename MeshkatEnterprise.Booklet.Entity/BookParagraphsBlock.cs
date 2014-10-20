using System.Collections.Generic;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class BookParagraphsBlock
    {
        public List<BookParagraph> Paragraphs{ get; set; }
        public List<BookComment> Comments{ get; set; }
        public List<BookHighlight> Highlights{ get; set; }
        public List<BookSectionStyle> Styles { get; set; } 
    }
}