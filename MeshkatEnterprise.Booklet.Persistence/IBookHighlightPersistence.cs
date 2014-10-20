using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookHighlightPersistence
    {
        long AddHighlight(BookHighlight highlight);
        List<BookHighlight> AddHighlight(List<BookHighlight> highlights);
        List<BookHighlight> GetBlockHighlights(long startParagraphId, long endParagraphId, long volumeId, long? personId);
        void RemoveHighlight(long highlightId);
        void RemoveParagraphHighlights(long paragraphId);
    }
}