using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookTableOfContentService
    {
        TServiceResult<List<BookTableOfContent>> GetChildren(long parentNodeId);
        TServiceResult<List<BookTableOfContent>> GetRoots();
        TServiceResult<Dictionary<string, string>> GetNodesTitleAndId();
        TServiceResult<string> GetPath(long nodeId);
        TServiceResult<List<BookTableOfContent>> GetSubTree(long id);

    }
}