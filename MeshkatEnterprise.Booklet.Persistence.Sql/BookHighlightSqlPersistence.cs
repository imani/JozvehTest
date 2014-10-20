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
    public class BookHighlightSqlPersistence : IBookHighlightPersistence
    {
        public long AddHighlight(BookHighlight highlight)
        {
            PersistenceContext persistence = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookHighlight_AddHighlight", (SqlConnection)persistence.Connection,
                persistence.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@paragraphId", highlight.HighlightSection.ParagraphId);
                command.Parameters.AddWithValue("@startOffset", highlight.HighlightSection.StartOffset);
                command.Parameters.AddWithValue("@endOffset", highlight.HighlightSection.EndOffset);
                command.Parameters.AddWithValue("@personId", highlight.PersonId);
                return (long)command.ExecuteScalar();
            }
        }

        public List<BookHighlight> GetBlockHighlights(long startParagraphId, long endParagraphId, long volumeId, long? personId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookHighlight_GetBlockHighlights", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@startParagraphId", startParagraphId);
                command.Parameters.AddWithValue("@endParagraphId", endParagraphId);
                command.Parameters.AddWithValue("@volumeId", volumeId);
                command.Parameters.AddWithValue("@personId", personId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new List<BookHighlight>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            var section = new Section();
                            section.ParagraphId = Convert.ToInt64(reader["ParagraphId"]);
                            section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                            section.EndOffset =Convert.ToInt32(reader["SectionEndOffset"]);
                            var highlight = new BookHighlight();
                            highlight.HighlightId = Convert.ToInt64(reader["HighlightId"]);
                            highlight.PersonId = Convert.ToInt64(reader["PersonId"]);
                            highlight.Color = reader["HighlightColor"].ToString();
                            highlight.HighlightSection = section;
                            result.Add(highlight);
                            
                        }
                    }

                    return result;
                }
            }
        }

        public void RemoveHighlight(long highlightId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookHighlight_RemoveHighlight", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@highlightId", highlightId);
                command.ExecuteScalar();
            }
        }


        public List<BookHighlight> AddHighlight(List<BookHighlight> highlights)
        {
            var result = new List<BookHighlight>();
            List<long> paragraphIds = new List<long>();
            foreach (var bookHighlight in highlights)
            {
                if (!paragraphIds.Contains(bookHighlight.HighlightSection.ParagraphId))
                {
                    var id = bookHighlight.HighlightSection.ParagraphId;
                    paragraphIds.Add(id);
                    this.RemoveParagraphHighlights(id);
                }
                var highlight = bookHighlight;
                highlight.HighlightId = this.AddHighlight(bookHighlight);
                result.Add(highlight);
            }
            return result;
        }


        public void RemoveParagraphHighlights(long paragraphId)
        {
            PersistenceContext persistence = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookHighlight_RemoveParagraphHighlights", (SqlConnection)persistence.Connection,
                persistence.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@paragraphId", paragraphId);
                command.ExecuteScalar();
            }
        }
    }
}