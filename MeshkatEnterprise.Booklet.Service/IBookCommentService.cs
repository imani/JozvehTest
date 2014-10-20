using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookCommentService
    {
        TServiceResult<long> AddComment(BookComment comment);

        ServiceResult RemoveComment(long bookCommentId);

        ServiceResult EditComment(BookComment comment);
    }
}