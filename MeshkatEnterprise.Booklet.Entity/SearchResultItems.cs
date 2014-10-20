using System;
using System.Collections.Generic;


namespace MeshkatEnterprise.Booklet.Entity
{
    public class SearchResultItems
    {
        public List<BookParagraph> Items { get; set; }
        public int TotalHits { get; set; }

        public SearchResultItems()
        {
            Items = new List<BookParagraph>();
        }

        public SearchResultItems(List<BookParagraph> results, int hits)
        {
            Items = results;
            TotalHits = hits;
        }
    }
}
