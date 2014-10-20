using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookVolumePersistence
    {
        BookVolume GetVolume(long volumeId);
    }
}