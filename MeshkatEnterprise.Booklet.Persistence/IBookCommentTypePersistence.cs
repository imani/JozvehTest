﻿using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookCommentTypePersistence
    {
        List<BookCommentType> GetBookCommentTypes(long bookId);
    }
}