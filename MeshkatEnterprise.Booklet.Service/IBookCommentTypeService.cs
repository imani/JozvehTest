using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookCommentTypeService
    {
        TServiceResult<List<BookCommentType>> GetCommentTypes(long bookId);
    }
}