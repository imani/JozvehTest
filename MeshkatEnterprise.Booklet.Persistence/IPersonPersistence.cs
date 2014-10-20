using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IPersonPersistence
    {
        List<Person> GetPersons(string[] personIdentities, string firstNamePattern, string lastNamePattern);
        List<Person> SearchPersons(string searchContent);
        long? GetPersonId(string personIdentity);
    }
}