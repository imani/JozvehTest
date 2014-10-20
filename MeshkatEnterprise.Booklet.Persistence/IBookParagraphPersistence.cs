using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookParagraphPersistence
    {
        Dictionary<long, BookParagraph> GetAllParagraphs();
        List<BookParagraph> GetBlockParagraphs(long startParagraphId, long endParagraphId, long volumeId);
        long GetParagraphId(long volumeId, int pageNumber);
    }
}