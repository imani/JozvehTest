using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class BookVolumeServiceImpl : IBookVolumeService
    {
        private readonly IBookVolumePersistence _bookVolumePersistence;

        public BookVolumeServiceImpl(IBookVolumePersistence bookVolumePersistence)
        {
            _bookVolumePersistence = bookVolumePersistence;
        }

        public TServiceResult<BookVolume> GetVolume(long volumeId)
        {
            return new TServiceResult<BookVolume>(_bookVolumePersistence.GetVolume(volumeId));
        }
    }
}