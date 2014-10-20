using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Vectorhighlight;
using Lucene.Net.Store;
using MeshkatEnterprise.Booklet.Entity;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace MeshkatEnterprise.Booklet.Search
{
    public class Searcher
    {
        public ArabicAnalyzerPlus Analyzer;
        public QueryParser ExactTextParser;
        public QueryWrapperFilter FilenameFilter;
        public IndexSearcher IndexSearcher;
        public QueryParser TextParser;
        public Directory IndexDirectory;

        /*public Searcher(IndexReader reader, String dataPath)
        {
            IndexSearcher = new IndexSearcher(reader);
            string[] stopwords = File.ReadAllLines(dataPath + "stopwords.txt", Encoding.UTF8);
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

            Analyzer = new ArabicAnalyzerPlus(Version.LUCENE_30, stopHashst);
            TextParser = new QueryParser(Version.LUCENE_30, "text", Analyzer);
            ExactTextParser = new QueryParser(Version.LUCENE_30, "exactText",
                new ArabicAnalyzerPlus(Version.LUCENE_30, stopHashst, false));
        }*/

        public Searcher(String indexPath, String dataPath)
        {
            IndexSearcher = new IndexSearcher(FSDirectory.Open(indexPath), true);
            string[] stopwords = File.ReadAllLines(dataPath + "stopwords.txt", Encoding.UTF8);
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

            Analyzer = new ArabicAnalyzerPlus(Version.LUCENE_30, stopHashst);
            TextParser = new QueryParser(Version.LUCENE_30, "text", Analyzer);
            ExactTextParser = new QueryParser(Version.LUCENE_30, "exactText",
                new ArabicAnalyzerPlus(Version.LUCENE_30, stopHashst, false));
        }

        public SearchResultItems Search(String query, int start, int end)
        {
            var results = new SearchResultItems();

            string querytext;

            if (start == 0 && query.Contains("xor"))
            {
                Match xorMath = Regex.Match(query, "([^\\s]*) xor ([^\\s]*)");
                string[] separators = {" xor "};
                string[] strs = xorMath.Value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                querytext = query.Replace(xorMath.Value, string.Format("(+({0} {1})-(+{0}+{1}))", strs[0], strs[1]));
            }
            else
            {
                querytext = query;
            }

            Query textQuery = TextParser.Parse(querytext);
            Query exactTextQuery = ExactTextParser.Parse(querytext);
            var booleanquery = new BooleanQuery();
            booleanquery.Add(textQuery, Occur.MUST);
            booleanquery.Add(exactTextQuery, Occur.SHOULD);
            TopDocs topdocs = IndexSearcher.Search(booleanquery, end);
            var highlighter = new FastVectorHighlighter();
            results.TotalHits = topdocs.TotalHits;
            try
            {
                for (int i = start; i < end; i++)
                {
                    ScoreDoc res = topdocs.ScoreDocs[i];
                    Document resDoc = IndexSearcher.Doc(res.Doc);
                    var resultParagraph = new BookParagraph();
                    resultParagraph.ParagraphId = long.Parse(resDoc.GetField("id").StringValue);
                    resultParagraph.VolumeInfo = new BookVolume();
                    resultParagraph.VolumeInfo.VolumeId = long.Parse(resDoc.GetField("VolumeId").StringValue);
                    resultParagraph.VolumeInfo.VolumeNumber = int.Parse(resDoc.GetField("VolumeNumber").StringValue);
                    resultParagraph.VolumeInfo.Book = new Book();
                    resultParagraph.VolumeInfo.Book.BookName = resDoc.GetField("bookName").StringValue;
                    resultParagraph.ParagraphPageNumber = int.Parse(resDoc.GetField("page").StringValue);
                    resultParagraph.TableOfContentNode = new BookTableOfContent();
                    resultParagraph.TableOfContentNode.Path = resDoc.GetField("tocPath").StringValue;
                    //retrieving snippet
                    string snippet = highlighter.GetBestFragment(highlighter.GetFieldQuery(booleanquery),
                        IndexSearcher.IndexReader,
                        res.Doc, "text", 100);
                    snippet = Regex.Replace(snippet, "</b>([^\n]{0,3})<b>", "$1");
                    snippet = snippet.Replace("<b>", "<b class='highlight'>");
                    resultParagraph.ParagraphText = snippet;
                    results.Items.Add(resultParagraph);
                }
            }
            catch (IndexOutOfRangeException expc)
            {
                return results;
            }

            return results;
        }
    }
}