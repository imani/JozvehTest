using System;
using Compositional.Composer;
using MeshkatEnterprise.Infrastructure.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    [TestClass]
    public class PersonUnitTest
    {
        public PersonUnitTest()
        {
            ContextUtil.Init();
            LoginUtil.Login();
        }

        [TestMethod]
        public void GetPersonUnitTest()
        {
            ComponentContext context = ComponentContextBinder.Lookup();
            string personIdentity = "88102101";
            var personService = context.GetComponent<IPersonService>();
            var result = personService.GetPerson(personIdentity);
            Assert.IsTrue(result.Successful && result.ReturnValue != null);
            var person = result.ReturnValue;
            Assert.IsTrue(person.PersonLastName == "اجودی");
        }
    }
}
