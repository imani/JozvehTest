using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class BookCommentSqlPersistence : IBookCommentPersistence
    {
        public BookComment GetComment(long commentId)
        {
            throw new NotImplementedException();
        }

        public List<BookComment> GetParagraphComments(long paragraphId, long? userId)
        {
            throw new NotImplementedException();
        }

        public List<BookComment> GetBlockComments(long startParagraphId, long endParagraphId, long volumeId, long? userId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookComment_GetBlockComments", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@startParagraphId", startParagraphId);
                command.Parameters.AddWithValue("@endParagraphId", endParagraphId);
                command.Parameters.AddWithValue("@volumeId", volumeId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new List<BookComment>();
                    if (reader.HasRows)
                    {
                        long previousId = -1;
                        while (reader.Read())
                        {
                            long currentId = Convert.ToInt64(reader["CommentId"]);
                            var section = new Section();
                            section.ParagraphId = Convert.ToInt64(reader["ParagraphId"]);
                            section.StartOffset = Convert.ToInt32( reader["SectionStartOffset"]);
                            section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                            if (previousId == currentId)
                            {
                                result[result.Count - 1].Sections.Add(section);
                            }
                            else
                            {
                                previousId = currentId;
                                var comment = new BookComment();
                                comment.Id = currentId;
                                comment.PersonId = Convert.ToInt64(reader["PersonId"]);
                                comment.PersonName = String.Format("{0} {1}", (string) reader["PersonName"],
                                    (string) reader["PersonLastName"]);
                                comment.Text = (string) reader["CommentText"];
                                comment.Type = new BookCommentType();
                                comment.Type.BookCommentTypeId = Convert.ToInt64(reader["CommentTypeId"]);
                                comment.Type.BookCommentTypeTitle = (string) reader["CommentTypeTitle"];
                                comment.Type.BookCommentTypeColor = (string) reader["CommentTypeColor"];
                                comment.Sections = new List<Section> {section};
                                result.Add(comment);
                            }
                        }
                    }
                    return result;
                }
            }
        }

        public long AddComment(BookComment comment)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookComment_AddComment", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@commentText", comment.Text);
                command.Parameters.AddWithValue("@commentTypeId", comment.Type.BookCommentTypeId);
                command.Parameters.AddWithValue("@personId", comment.PersonId);
                comment.Id = (long)command.ExecuteScalar();
            }

            foreach (Section section in comment.Sections)
            {
                using (var command = new SqlCommand("SP_BookComment_AddCommentSection", (SqlConnection)persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@paragraphId", section.ParagraphId);
                    command.Parameters.AddWithValue("@sectionStartOffset", section.StartOffset);
                    command.Parameters.AddWithValue("@sectionEndOffset", section.EndOffset);
                    command.Parameters.AddWithValue("@commentId", comment.Id);
                    command.ExecuteScalar();
                }
            }
            return comment.Id;
        }

        public void EditComment(BookComment comment)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookComment_EditComment", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@commentId", comment.Id);
                command.Parameters.AddWithValue("@commentText", comment.Text);
                command.Parameters.AddWithValue("@commentTypeId", comment.Type.BookCommentTypeId);

                command.ExecuteScalar();
            }
        }

        public void RemoveComment(long commentId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookComment_RemoveComment", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@commentId", commentId);
                command.ExecuteScalar();
            }
        }
    }
}