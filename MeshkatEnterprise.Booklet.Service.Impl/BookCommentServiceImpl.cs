using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class BookCommentServiceImpl : IBookCommentService
    {
        private IBookCommentPersistence _bookCommentPersistence;
        private IPersonPersistence _personPersistence;

        public BookCommentServiceImpl(IBookCommentPersistence bookCommentPersistence, IPersonPersistence personPersistence)
        {
            _bookCommentPersistence = bookCommentPersistence;
            _personPersistence = personPersistence;
        }
        public TServiceResult<long> AddComment(BookComment comment)
        {
            comment.PersonId = Convert.ToInt16(_personPersistence.GetPersonId(SecurityTokenBinder.Lookup().UserName));
            comment.PersonName = SecurityTokenBinder.Lookup().UserName;
            return new TServiceResult<long>(_bookCommentPersistence.AddComment(comment));
        }

        public ServiceResult RemoveComment(long bookCommentId)
        {
            _bookCommentPersistence.RemoveComment(bookCommentId);
            return new ServiceResult();
        }

        public ServiceResult EditComment(BookComment comment)
        {
            _bookCommentPersistence.EditComment(comment);
            return new ServiceResult();
        }
    }
}