using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookHighlightService
    {
        TServiceResult<List<BookHighlight>> AddHighlight(List<BookHighlight> hl);

        ServiceResult RemoveHighlight(long highlightId);
    }
}