using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Booklet.Search;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    /**
    * Search Service class
    * 
    * This Class provided searching and indexing services for search engine
    * @see MeshkatEnterprise.Booklet.Service.Dummy.Search
    * @see MeshkatEnterprise.Booklet.Entity
    * 
    * @author Mohsen Imany
    */

    [Component]
    public class SearchServiceImpl : ISearchService
    {
        private readonly string DATA_PATH;
        private readonly string INDEX_PATH;
        private readonly Indexer _indexer;
        private readonly ISearchPersistence _searchPersistence;
        private readonly Searcher _searcher;


        [CompositionConstructor]
        /** Default constructor
         */
        public SearchServiceImpl(ISearchPersistence searchPersistence)
        {
            _searchPersistence = searchPersistence;
            DATA_PATH = ComponentContextBinder.Lookup().GetVariable("DATA").ToString();
            INDEX_PATH = ComponentContextBinder.Lookup().GetVariable("INDEX").ToString();
            _searcher = new Searcher(INDEX_PATH, DATA_PATH);
        }


        /** Search Method
       * 
       * Search the given query and returns results from start to end
       * this method calls MeshkatEnterprise.Booklet.Service.Dummy.Search.searcher.Search()
       * @param query Query String
       * @param start Start index of search results
       * @param end End index of search results
       * @return An instance of MeshkatEnterprise.Booklet.Entity.SearchResultItems
       * @see MeshkatEnterprise.Booklet.Service.Impl.Search
       */

        public TServiceResult<SearchResultItems> Search(string query, int start, int end)
        {
            /*
            SearchResultItems ResultItems = new SearchResultItems();
            List<BookParagraph> SearchResults = new List<BookParagraph>();

            BookTableOfContent TOC1 = new BookTableOfContent { BookParagraphId = 1, IsLazy = false };
            TOC1.BookParagraphId = 1;
            TOC1.HasChild = false;
            TOC1.Path = "1\\2\\3";
            TOC1.Title = "المقدمه";
            TOC1.Key = 458489;

            Book Bk1 = new Book();
            Bk1.BookId = 1;
            Bk1.BookName = "فقه";

            BookVolume VOL1 = new BookVolume();
            VOL1.Book = Bk1;
            VOL1.VolumeId = 1;
            VOL1.VolumeNumber = 2;

            BookParagraph res1 = new BookParagraph();
            res1.ParagraphId = 1;
            res1.ParagraphPageNumber = 23;
            res1.ParagraphText = "پیش از این حجت الاسلام رئیسی در سمت معاون اول قوه قضاییه و حجت الاسلام اژه‌ای در سمت دادستان کل کشور فعالیت می‌کردند و به این ترتیب با احکام جدید رئیس قوه قضائیه، هر کدام جایگزین دیگری شدند. ";
            res1.TableOfContentNode = TOC1;
            res1.VolumeInfo = VOL1;


            BookTableOfContent TOC2 = new BookTableOfContent { BookParagraphId = 1 };
            TOC2.BookParagraphId = 2;
            TOC2.HasChild = false;
            TOC2.IsLazy = false;
            TOC2.Path = "1\\2\\3\\1";
            TOC2.Title = "المقدمه";
            TOC2.Key = 458489;

            Book Bk2 = new Book();
            Bk2.BookId = 2;
            Bk2.BookName = "اللغه";

            BookVolume VOL2 = new BookVolume();
            VOL2.Book = Bk2;
            VOL2.VolumeId = 2;
            VOL2.VolumeNumber = 3;

            BookParagraph res2 = new BookParagraph();
            res2.ParagraphId = 100;
            res2.ParagraphPageNumber = 43;
            res2.ParagraphText = "آژانس بین‌المللی انرژی اتمی امروز چهارشنبه با انتشار گزارشی، از پایبند بودن ایران به شرایط توافق موقت ژنو خبر خواهد داد. ";
            res2.TableOfContentNode = TOC2;
            res2.VolumeInfo = VOL2;


            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res1);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res2);
            SearchResults.Add(res2);


            ResultItems.Items = SearchResults;
            ResultItems.TotalHits = ResultItems.Items.Count;
            return new TServiceResult<SearchResultItems>(ResultItems);
             **/
            var result = new TServiceResult<SearchResultItems>(_searcher.Search(query, start, end));
            return result;
        }
    }
}