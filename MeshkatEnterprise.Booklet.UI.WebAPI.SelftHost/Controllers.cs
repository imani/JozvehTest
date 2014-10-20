using System.Collections.Generic;
using System.Web.Http;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Service;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.UI.WebAPI.SelfHost
{
    public class BookTableOfContentController : ApiController, IBookTableOfContentService
    {
        public TServiceResult<List<BookTableOfContent>> GetChildren(long parentNodeId)
        {
            return ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetChildren(parentNodeId);
        }

        public TServiceResult<List<BookTableOfContent>> GetRoots()
        {
            return ComponentContextBinder.Lookup().GetComponent<IBookTableOfContentService>().GetRoots();
        }
    }

    public class BookVolumeController : ApiController, IBookVolumeService
    {
        public TServiceResult<BookVolume> GetVolume(long volumeId)
        {
            return ComponentContextBinder.Lookup().GetComponent<IBookVolumeService>().GetVolume(volumeId);
        }
    }
}