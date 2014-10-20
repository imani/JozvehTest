using System;
using System.Data;
using System.Data.SqlClient;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class BookVolumeSqlPersistence : IBookVolumePersistence
    {
        public BookVolume GetVolume(long volumeId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (
                var command = new SqlCommand("SP_BookVolume_GetVolume", (SqlConnection) persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@volumeId", volumeId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BookVolume result = null;

                    if (reader.HasRows)
                    {
                        result = new BookVolume();

                        while (reader.Read())
                        {
                            var book = new Book();
                            book.BookId = (long) reader["BookId"];
                            book.BookName = (String) reader["BookTitle"];
                            result.Book = book;
                            result.VolumeId = (long) reader["VolumeId"];
                            result.VolumeNumber = (long) reader["VolumeNumber"];
                            result.StartParagraphId = (long) reader["StartParagraphId"];
                            result.EndParagraphId = (long) reader["EndParagraphId"];
                            result.Pages = (int) reader["Pages"];
                        }
                    }

                    return result;
                }
            }
        }

        public void GetStartAndEndParagraph(out long startParagraph, out long endParagraph)
        {
            startParagraph = -1;
            endParagraph = -1;

            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (
                var command = new SqlCommand("SP_BookVolume_GetStartAndEndParagraph",
                    (SqlConnection) persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            startParagraph = (long) reader["StartParagraph"];
                            endParagraph = (long) reader["EndParagraph"];
                        }
                    }
                }
            }
        }
    }
}