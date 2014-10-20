using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class BookCommentTypeServiceImpl : IBookCommentTypeService
    {
        public TServiceResult<List<BookCommentType>> GetCommentTypes(long bookId)
        {
            throw new NotImplementedException();
        }
    }
}