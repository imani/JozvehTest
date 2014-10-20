using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Infrastructure.General.ServiceResponse;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class BookHighlightServiceImpl : IBookHighlightService
    {
        private readonly IBookHighlightPersistence _bookHighlightPersistence;
        private readonly IPersonPersistence _personPersistence;

        public BookHighlightServiceImpl(IBookHighlightPersistence bookHighlightPersistence, IPersonPersistence personPersistence)
        {
            _bookHighlightPersistence = bookHighlightPersistence;
            _personPersistence = personPersistence;
        }

        public TServiceResult<List<BookHighlight>> AddHighlight(List<BookHighlight> hl)
        {
            if (hl == null)
            {
                throw new InvalidOperationException("no highlight to add");
            }
            foreach (var bookHighlight in hl)
            {
                bookHighlight.PersonId = Convert.ToInt16(_personPersistence.GetPersonId(SecurityTokenBinder.Lookup().UserName));
            }
            return new TServiceResult<List<BookHighlight>>(_bookHighlightPersistence.AddHighlight(hl));
        }

        public ServiceResult RemoveHighlight(long highlightId)
        {
            _bookHighlightPersistence.RemoveHighlight(highlightId);
            return new ServiceResult();
        }
    }
}