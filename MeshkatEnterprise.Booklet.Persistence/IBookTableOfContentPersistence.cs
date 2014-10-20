using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookTableOfContentPersistence
    {
        List<BookTableOfContent> GetRoots();
        List<BookTableOfContent> GetChildren(long tableOfContentId);
        Dictionary<string, string> GetNodesTitleAndId();
        string GetPath(long nodeId);
        List<BookTableOfContent> GetSubTree(long tableOfContentId);
    }
}