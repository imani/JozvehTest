//using Compositional.Composer;
//using MeshkatEnterprise.Booklet.Entity;
//using MeshkatEnterprise.Booklet.Persistence;
//using MeshkatEnterprise.Infrastructure.General;
//using Newtonsoft.Json;

//namespace MeshkatEnterprise.Booklet.Service.Impl
//{
//    [Component]
//    public class UserPreferenceServiceImpl : IUserPreferenceService
//    {
//        private readonly IUserPreferencePersistence _userPreferencePersistence;

//        public UserPreferenceServiceImpl(IUserPreferencePersistence userPreferencePersistence)
//        {
//            _userPreferencePersistence = userPreferencePersistence;
//        }

//        public ServiceResult SavePreference(UserPreference preference)
//        {
//            _userPreferencePersistence.RemovePreference(preference.PersonId);
//            _userPreferencePersistence.AddPreference(preference.PersonId, JsonConvert.SerializeObject(preference.State));

//            return new ServiceResult();
//        }

//        public TServiceResult<string> LoadPreference(long personId)
//        {
//            var preference = JsonConvert.DeserializeObject<UserPreference>(_userPreferencePersistence.GetPreference(personId));

//            return new TServiceResult<string>(JsonConvert.SerializeObject(preference));

//        }
//    }
//}

