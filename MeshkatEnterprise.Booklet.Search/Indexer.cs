using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using MeshkatEnterprise.Booklet.Entity;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace MeshkatEnterprise.Booklet.Search
{
    internal enum TextTypeSet
    {
        Text,
        Title,
        Coment
    }

    public class Indexer
    {
        private readonly ArabicAnalyzerPlus _analyzer;
        private readonly String _dataDir;
        private readonly Directory _indexDir;
        private readonly PerFieldAnalyzerWrapper _perfieldAnalyzer;
        private readonly ArabicAnalyzerPlus _simpleAnalyzer;
        private readonly IndexWriter _writer;

        public Indexer(String indexPath, String dataPath)
        {
            _indexDir = FSDirectory.Open(indexPath);
            _dataDir = dataPath;

            // create stopwords hash set
            string[] stopwords = File.ReadAllLines(_dataDir + "stopwords.txt", Encoding.UTF8);
            var stopHashst = new HashSet<string>();
            for (int i = 0; i < stopwords.Length; i++)
            {
                try
                {
                    stopHashst.Add(stopwords[i]);
                }
                catch (Exception)
                {
                }
            }

            //initialize analyzers
            _analyzer = new ArabicAnalyzerPlus(Version.LUCENE_CURRENT, stopHashst);
            _simpleAnalyzer = new ArabicAnalyzerPlus(Version.LUCENE_CURRENT, stopHashst, false);
            _perfieldAnalyzer = new PerFieldAnalyzerWrapper(_analyzer);
            _perfieldAnalyzer.AddAnalyzer("exactText", _simpleAnalyzer);

            //initialize index writer
            _writer = new IndexWriter(_indexDir, _perfieldAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        /*
         * index method for one document
         */

        public void Index(BookParagraph paragraph)
        {
            var doc = new Document();
            doc.Add(new Field("text", paragraph.ParagraphText, Field.Store.YES, Field.Index.ANALYZED,
                Field.TermVector.WITH_POSITIONS_OFFSETS));
            doc.Add(new Field("id", paragraph.ParagraphId.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("VolumeId", paragraph.VolumeInfo.VolumeId.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("VolumeNumber", paragraph.VolumeInfo.VolumeNumber.ToString(), Field.Store.YES,
                Field.Index.NO));
            doc.Add(new Field("bookName", paragraph.VolumeInfo.Book.BookName, Field.Store.YES,
                Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("page", paragraph.ParagraphPageNumber.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("tocPath", paragraph.TableOfContentNode.Path, Field.Store.YES, Field.Index.NO));
            _writer.AddDocument(doc);
            _writer.Commit();
        }

        /*
         * index method for a list of documents
         */

        //public void Index(List<DocInformation> list)
        //{
        //    foreach (DocInformation doc in list)
        //        Index(doc);
        //    _writer.Optimize();
        //}

        public IndexReader GetReader()
        {
            return _writer.GetReader();
        }

        //public void RemoveIndex(long id)
        //{
        //    _writer.DeleteDocuments(new Term("id", id.ToString()));
        //    _writer.Commit();
        //}

        //public void UpdateIndex(DocInformation doc)
        //{
        //    RemoveIndex(doc.Id);
        //    Index(doc);
        //}
    }
}