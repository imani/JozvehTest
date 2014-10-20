using System;
using Compositional.Composer;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IUserPreferencePersistence
    {
        void AddPreference(long personId, String stateJson);
        void RemovePreference(long personId);
        String GetPreference(long personId);
    }
}