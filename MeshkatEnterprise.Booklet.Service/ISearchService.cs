using System;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface ISearchService
    {
        TServiceResult<SearchResultItems> Search(String query, int start, int end);
    }
}