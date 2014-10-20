using System.Collections.Generic;
using System.Linq;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class PersonServiceImpl : IPersonService
    {
        private readonly IPersonPersistence _personPersistence;

        public PersonServiceImpl(IPersonPersistence personPersistence)
        {
            _personPersistence = personPersistence;
        }
        public TServiceResult<Person> GetPerson(string personIdentity)
        {
            List<Person> resultsList = _personPersistence.GetPersons(new string[]{personIdentity}, null, null);
            return new TServiceResult<Person>(resultsList.First());
        }
    }
}