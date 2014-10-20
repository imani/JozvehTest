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
    public class BookParagraphSqlPersistence : IBookParagraphPersistence
    {
        public Dictionary<long, BookParagraph> GetAllParagraphs()
        {
            var ret = new Dictionary<long, BookParagraph>();
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (
                var command = new SqlCommand("SP_BookParagraph_GetAllParagraphs",
                    (SqlConnection) persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        long previousId = -1;
                        while (reader.Read())
                        {
                            var item = new BookParagraph();
                            long currentId = (long)reader["ParagraphId"];
                            if (previousId == currentId)
                            {
                                Footnote footnote = new Footnote();
                                footnote.Id = Convert.ToInt64(reader["FootnoteId"]);
                                footnote.Text = reader["FootnoteText"].ToString();
                                footnote.Section = new Section();
                                footnote.Id = Convert.ToInt64(reader["SectionId"]);
                                footnote.Section.ParagraphId = item.ParagraphId;
                                footnote.Section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                                footnote.Section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                                item.Footnotes.Add(footnote);
                            }
                            else
                            {
                                item.ParagraphId = currentId;
                                item.ParagraphPageNumber = (int)reader["PageNumber"];
                                item.ParagraphText = (String)reader["ParagraphText"];
                                var volume = new BookVolume();
                                volume.VolumeId = (long)reader["VolumeId"];
                                volume.VolumeNumber = (int)reader["VolumeNumber"];
                                volume.Book = new Book();
                                volume.Book.BookId = (long)reader["BookId"];
                                volume.Book.BookName = (String)reader["BookName"];
                                item.VolumeInfo = volume;
                                var toc = new BookTableOfContent();
                                toc.BookParagraphId = item.ParagraphId;
                                toc.VolumeId = item.VolumeInfo.VolumeId;
                                toc.Key = (long)reader["TableOfContentId"];
                                toc.ParentKey = (long)reader["TableOfContentParentId"];
                                toc.Title = (String)reader["TableOfContentTitle"];
                                toc.Path = (String)reader["Path"];
                                if ((int)reader["childrenCount"] > 0)
                                    toc.HasChild = true;
                                else
                                {
                                    toc.HasChild = false;
                                }
                                item.TableOfContentNode = toc;
                                item.Footnotes = new List<Footnote>();
                                if (reader["FootnoteId"] != null)
                                {
                                    Footnote footnote = new Footnote();
                                    footnote.Id = Convert.ToInt64(reader["FootnoteId"]);
                                    footnote.Text = reader["FootnoteText"].ToString();
                                    footnote.Section = new Section();
                                    footnote.Id = Convert.ToInt64(reader["SectionId"]);
                                    footnote.Section.ParagraphId = item.ParagraphId;
                                    footnote.Section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                                    footnote.Section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                                    item.Footnotes.Add(footnote);
                                }
                            }
                            

                            ret.Add(item.ParagraphId, item);
                        }
                    }
                }

                return ret;
            }
        }

        public List<BookParagraph> GetBlockParagraphs(long startParagraphId, long endParagraphId, long volumeId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (
                var command = new SqlCommand("SP_BookParagraph_GetBlockParagraphs",
                    (SqlConnection) persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@startParagraphId", startParagraphId);
                command.Parameters.AddWithValue("@endParagraphId", endParagraphId);
                command.Parameters.AddWithValue("@volumeId", volumeId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var paragraphs = new List<BookParagraph>();
                    if (reader.HasRows)
                    {
                        long previousId = -1;
                        var item = new BookParagraph();
                        while (reader.Read())
                        {
                            long currentId = (long)reader["ParagraphId"];
                            if (currentId == previousId)
                            {
                                Footnote footnote = new Footnote();
                                footnote.Id = Convert.ToInt64(reader["FootnoteId"]);
                                footnote.Text = reader["FootnoteText"].ToString();
                                footnote.Section = new Section();
                                footnote.Id = Convert.ToInt64(reader["SectionId"]);
                                footnote.Section.ParagraphId = item.ParagraphId;
                                footnote.Section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                                footnote.Section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                                item.Footnotes.Add(footnote);
                                paragraphs.RemoveAt(paragraphs.Count - 1);
                                paragraphs.Add(item);
                            }
                            else
                            {
                                previousId = currentId;
                                item = new BookParagraph();
                                item.ParagraphId = currentId;
                                item.ParagraphPageNumber = (int)reader["ParagraphPageNumber"];
                                item.ParagraphText = (String)reader["ParagraphText"];
                                var volume = new BookVolume();
                                volume.VolumeId = (long)reader["VolumeId"];
                                volume.VolumeNumber = (long)reader["VolumeNumber"];
                                volume.Book = new Book();
                                volume.Book.BookId = (long)reader["BookId"];
                                volume.Book.BookName = (String)reader["BookTitle"];
                                item.VolumeInfo = volume;
                                var toc = new BookTableOfContent();
                                toc.BookParagraphId = item.ParagraphId;
                                toc.VolumeId = item.VolumeInfo.VolumeId;
                                toc.Key = (long)reader["TableOfContentId"];
                                toc.ParentKey = reader["TableOfContentParentId"] as long? ?? null;
                                toc.Title = (String)reader["TableOfContentTitle"];
                                toc.Path = (String)reader["Path"];
                                if ((int)reader["childrenCount"] > 0)
                                    toc.HasChild = true;
                                else
                                {
                                    toc.HasChild = false;
                                }
                                item.TableOfContentNode = toc;
                                item.Footnotes = new List<Footnote>();
                                if (reader["FootnoteId"] != DBNull.Value)
                                {
                                    Footnote footnote = new Footnote();
                                    footnote.Id = Convert.ToInt64(reader["FootnoteId"]);
                                    footnote.Text = reader["FootnoteText"].ToString();
                                    footnote.Section = new Section();
                                    footnote.Id = Convert.ToInt64(reader["SectionId"]);
                                    footnote.Section.ParagraphId = item.ParagraphId;
                                    footnote.Section.StartOffset = Convert.ToInt32(reader["SectionStartOffset"]);
                                    footnote.Section.EndOffset = Convert.ToInt32(reader["SectionEndOffset"]);
                                    item.Footnotes.Add(footnote);
                                }
                                paragraphs.Add(item);
                            }                            
                        }
                    }
                    return paragraphs;
                }
            }
        }

        public long GetParagraphId(long volumeId, int pageNumber)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (
                var command = new SqlCommand("SP_BookParagraph_GetPageParagraphId",
                    (SqlConnection)persistenceContext.Connection,
                    persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@volumeId", volumeId);
                command.Parameters.AddWithValue("@pageNumber", pageNumber);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    long result = -1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = Convert.ToInt64(reader["ParagraphId"]);
                        }
                    }
                    return result;


                }
            }
        }
    }
}