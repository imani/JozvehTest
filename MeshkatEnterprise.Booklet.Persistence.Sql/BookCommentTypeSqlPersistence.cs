using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class BookCommentTypeSqlPersistence : IBookCommentTypePersistence
    {
        public List<BookCommentType> GetBookCommentTypes(long bookId)
        {
            List<BookCommentType> result = null;
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookCommentType_GetCommentTypes", (SqlConnection)persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@bookId", bookId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = new List<BookCommentType>();
                        long previousRowId = 0;
                        while (reader.Read())
                        {
                            var currentRowId = (long)reader["CommentTypeId"];
                            if (previousRowId == currentRowId)
                            {
                                BookCommentType item = result.Last();
                                result.Remove(item);
                                long commentFieldId = item.BookCommentFields.Last().BookCommentFieldId;
                                if (commentFieldId != (long)reader["CommentFieldId"])
                                {
                                    var field = new BookCommentField();
                                    field.BookCommentFieldId = (long)reader["CommentFieldId"];
                                    field.BookCommentFieldTitle = (string)reader["CommentFieldTitle"];
                                    item.BookCommentFields.Add(field);
                                }
                                result.Add(item);
                            }
                            else
                            {
                                previousRowId = currentRowId;
                                var item = new BookCommentType();
                                item.BookId = bookId;
                                item.BookCommentTypeId = currentRowId;
                                item.BookCommentTypeColor = (string)reader["CommentTypeColor"];
                                item.BookCommentTypeTitle = (string)reader["CommentTypeTitle"];
                                if (reader["CommentFieldId"] != DBNull.Value)
                                {
                                    var field = new BookCommentField();
                                    field.BookCommentFieldId = (long)reader["CommentFieldId"];
                                    field.BookCommentFieldTitle = (string)reader["CommentFieldTitle"];
                                    item.BookCommentFields.Add(field);
                                }
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}