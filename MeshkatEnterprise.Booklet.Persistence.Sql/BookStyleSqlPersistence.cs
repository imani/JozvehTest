using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;
using System;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class BookStyleSqlPersistence : IBookStylePersistence
    {
        public List<BookSectionStyle> GetBlockStyles(long startParagraphId, long endParagraphId, long volumeId)
        {
            List<BookSectionStyle> ret = null;
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();

            using (
                var command = new SqlCommand("SP_BookStyle_GetBlockStyles",
                    (SqlConnection)persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@startParagraphId", startParagraphId);
                command.Parameters.AddWithValue("@endParagraphId", endParagraphId);
                command.Parameters.AddWithValue("@volumeId", volumeId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ret = new List<BookSectionStyle>();
                        while (reader.Read())
                        {
                            var item = new BookSectionStyle();
                            var section = new Section();
                            section.ParagraphId = Convert.ToInt64(reader["ParagraphId"]);
                            section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                            section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                            item.Section = section;
                            item.Style = reader["Style"] as String;
                            ret.Add(item);
                        }
                    }
                }
            }
            return ret;
        }

        public List<BookSectionStyle> GetAllStyles()
        {
            List<BookSectionStyle> ret = null;
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();

            using (
                var command = new SqlCommand("SP_BookStyle_GetAllStyles", (SqlConnection)persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ret = new List<BookSectionStyle>();
                        while (reader.Read())
                        {
                            var item = new BookSectionStyle();
                            var section = new Section();
                            section.ParagraphId = Convert.ToInt64(reader["ParagraphId"]);
                            section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                            section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                            item.Section = section;
                            item.Style = (string)reader["Style"];
                            ret.Add(item);
                        }
                    }
                }
            }
            return ret;
        }
    }
}