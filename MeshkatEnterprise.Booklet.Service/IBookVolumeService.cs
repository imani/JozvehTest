using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookVolumeService
    {
        TServiceResult<BookVolume> GetVolume(long volumeId);
    }
}