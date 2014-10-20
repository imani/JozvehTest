using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookParagraphService
    {
        TServiceResult<BookParagraphsBlock> GetParagraphsBlock(long startParagraphId, long endParagraphId, long volumeId);

        TServiceResult<BookParagraphsBlock> GetParagraphsByPageNumber(long volumeId, int pageNumber, int fetchSizeBefore, int fetchSizeAfter);
    }
}