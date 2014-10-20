using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class BookCommentTypeServiceImpl : IBookCommentTypeService
    {
        private readonly IBookCommentTypePersistence _bookCommentTypePersistence;

        public BookCommentTypeServiceImpl(IBookCommentTypePersistence bookCommentTypePersistence)
        {
            _bookCommentTypePersistence = bookCommentTypePersistence;
        }
        public TServiceResult<List<BookCommentType>> GetCommentTypes(long bookId)
        {
            return new TServiceResult<List<BookCommentType>>(_bookCommentTypePersistence.GetBookCommentTypes(bookId));
        }
    }
}