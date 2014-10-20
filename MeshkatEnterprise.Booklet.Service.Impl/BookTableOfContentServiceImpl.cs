using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.Impl 
{
    [Component]
    public class BookTableOfContentServiceImpl : IBookTableOfContentService
    {
        private readonly IBookTableOfContentPersistence _bookTableOfContentPersistence;
        public BookTableOfContentServiceImpl(IBookTableOfContentPersistence bookTableOfContentPersistence)
        {
            _bookTableOfContentPersistence = bookTableOfContentPersistence;
        }
        public TServiceResult<List<BookTableOfContent>> GetRoots()
        {
            return new TServiceResult<List<BookTableOfContent>>(_bookTableOfContentPersistence.GetRoots());
        }

        public TServiceResult<Dictionary<string, string>> GetNodesTitleAndId()
        {
            return new TServiceResult<Dictionary<string, string>>(_bookTableOfContentPersistence.GetNodesTitleAndId());
        }

        public TServiceResult<string> GetPath(long nodeId)
        {
            return new TServiceResult<string>(_bookTableOfContentPersistence.GetPath(nodeId));
        }

        public TServiceResult<List<BookTableOfContent>> GetChildren(long parentNodeId)
        {
            return new TServiceResult<List<BookTableOfContent>>(_bookTableOfContentPersistence.GetChildren(parentNodeId));
        }


        public TServiceResult<List<BookTableOfContent>> GetSubTree(long id)
        {
            return new TServiceResult<List<BookTableOfContent>>(_bookTableOfContentPersistence.GetSubTree(id));
        }
    }
}
