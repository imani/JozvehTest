﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.Interception.Persistence;

namespace MeshkatEnterprise.Booklet.Persistence.Sql
{
    [Component]
    public class BookTableOfContentSqlPersistence : IBookTableOfContentPersistence
    {
        public List<BookTableOfContent> GetRoots()
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookTableOfContent_GetRoots",
                (SqlConnection) persistenceContext.Connection, persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new List<BookTableOfContent>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new BookTableOfContent();
                            item.Key = (long) reader["TableOfContentId"];
                            item.ParentKey = reader["TableOfContentParentId"] as long? ?? null;
                            item.Title = (String) reader["TableOfContentTitle"];
                            item.BookParagraphId = (long) reader["ParagraphId"];
                            item.HasChild = (int) reader["ChildrenCount"] > 0;
                            item.VolumeId = (long) reader["VolumeId"];
                            item.Path = (string) reader["Path"];
                            result.Add(item);
                        }
                    }

                    return result;
                }
            }
        }


        public List<BookTableOfContent> GetChildren(long tableOfContentId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookTableOfContent_GetChildren",
                (SqlConnection) persistenceContext.Connection, persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@tableOfContentId", tableOfContentId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new List<BookTableOfContent>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new BookTableOfContent();
                            item.Key = (long) reader["TableOfContentId"];
                            item.ParentKey = reader["TableOfContentParentId"] as long? ?? null;
                            item.Title = (String) reader["TableOfContentTitle"];
                            item.BookParagraphId = (long) reader["ParagraphId"];
                            item.HasChild = (int) reader["ChildrenCount"] > 0;
                            item.VolumeId = (long) reader["VolumeId"];

                            result.Add(item);
                        }
                    }

                    return result;
                }
            }
        }

        public Dictionary<string, string> GetNodesTitleAndId()
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookTableOfContent_GetNodesTitleAndId", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new Dictionary<string, string>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string title = this.Normalize((string) reader["TableOfContentTitle"]);
                            string id = reader["TableOfContentId"].ToString();
                            result.Add(id, title);
                        }
                    }

                    return result;
                }
            }
        }


        public string GetPath(long nodeId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookTableOfContent_GetPath", (SqlConnection)persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    String result = null;
                    if (reader.HasRows && reader.Read())
                    {
                        result = (string) reader["Path"];
                    }
                    return result;
                }
            }
        }




        public List<BookTableOfContent> GetSubTree(long tableOfContentId)
        {
            PersistenceContext persistenceContext = PersistenceContextBinder.Lookup();
            using (var command = new SqlCommand("SP_BookTableOfContent_GetSubTree", (SqlConnection) persistenceContext.Connection,
                persistenceContext.Transaction as SqlTransaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tableOfContentId", tableOfContentId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new List<BookTableOfContent>();
                    
                    while (reader.HasRows && reader.Read())
                    {
                        var currentNode = new BookTableOfContent();
                        currentNode.Key = Convert.ToInt64(reader["TableOfContentId"]);
                        if (reader["TableOfContentParentId"] != DBNull.Value)
                            currentNode.ParentKey = Convert.ToInt64(reader["TableOfContentParentId"]);
                        currentNode.Title = (String)reader["TableOfContentTitle"];
                        currentNode.BookParagraphId = Convert.ToInt64(reader["ParagraphId"]);
                        currentNode.Path = (String) reader["Path"];
                        currentNode.VolumeId = Convert.ToInt64(reader["VolumeId"]);
                        currentNode.HasChild = ((int) reader["ChildrenCount"] > 0);
                        result.Add(currentNode);
                    }

                    return result;
                }
            }
        }


        private string Normalize(string text)
        {
            MakeTrans translations = new MakeTrans(" كي;%1234567890", " کی؛٪۱۲۳۴۵۶۷۸۹۰");
            return translations.Translate(text);
        }

    }

    internal class MakeTrans
    {
        private readonly Dictionary<char, char> d;
        public MakeTrans(string intab, string outab)
        {
            d = Enumerable.Range(0, intab.Length).ToDictionary(i => intab[i], i => outab[i]);
            //d = new Dictionary<char, char>();
            //for (int i = 0; i < intab.Length; i++)
            // d[intab[i]] = outab[i];
        }
        public string Translate(string src)
        {
            System.Text.StringBuilder sb = new StringBuilder(src.Length);
            foreach (char src_c in src)
                sb.Append(d.ContainsKey(src_c) ? d[src_c] : src_c);
            return sb.ToString();
        }
    }
}