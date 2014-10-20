using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookCommentPersistence
    {
        BookComment GetComment(long commentId);

        /// <summary>
        ///     get list of comments that belongs to a specific paragraph
        /// </summary>
        /// <param name="paragraphId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<BookComment> GetParagraphComments(long paragraphId, long? personId);

        List<BookComment> GetBlockComments(long startParagraphId, long endParagraphId, long volumeId, long? personId);

        long AddComment(BookComment comment);

        void EditComment(BookComment comment);

        void RemoveComment(long commentId);
    }
}