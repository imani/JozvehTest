using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class SearchSqlPersistence : ISearchPersistence
    {
        public List<BookParagraph> GetAllIndexDocuments()
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_GetAllIndexDocuments",
                (SqlConnection) persistenceContext.Connection, persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var docs = new List<BookParagraph>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var doc = new BookParagraph();
                            doc.ParagraphId = (long) reader["ParagraphId"];
                            doc.ParagraphPageNumber = (int) reader["ParagraphPageNumber"];
                            doc.ParagraphText = (string) reader["ParagraphText"];
                            doc.VolumeInfo = new BookVolume();
                            doc.VolumeInfo.Book = new Book();
                            doc.VolumeInfo.Book.BookName = (string) reader["BookTitle"];
                            doc.VolumeInfo.VolumeId = (long) reader["VolumeId"];
                            doc.VolumeInfo.VolumeNumber = (long) reader["VolumeNumber"];
                            doc.TableOfContentNode = new BookTableOfContent();
                            doc.TableOfContentNode.Path = (string) reader["Path"];
                            docs.Add(doc);
                        }
                    }

                    return docs;
                }
            }
        }
    }
}