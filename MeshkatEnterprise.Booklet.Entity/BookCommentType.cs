using System.Collections.Generic;
namespace MeshkatEnterprise.Booklet.Entity
{
    public class BookCommentType
    {
        public long BookCommentTypeId { get; set; }
        public string BookCommentTypeTitle { get; set; }
        public string BookCommentTypeColor { get; set; }
        public long BookId  { get; set; }
        public BookCommentSubject SubjectRoot{ get; set; }
        public List<BookCommentField> BookCommentFields { get; set; }

        public BookCommentType()
        {
            BookCommentFields = new List<BookCommentField>();
        }
    }
}
