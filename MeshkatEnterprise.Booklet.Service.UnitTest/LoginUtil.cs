using Compositional.Composer;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    public static class LoginUtil
    {
        public static void Login()
        {
            ComponentContext componentContext = ComponentContextBinder.Lookup();
            var securityService = componentContext.GetComponent<ISecurityService>();
            IAuthenticationTemplate userNamePasswordTemplate = new UserNamePasswordTemplate
            {
                UserName = "88102101",
                Password = "0942600428"
            };

            AuthenticationResponse response = securityService.Authenticate(userNamePasswordTemplate);
            SecurityTokenBinder.Bind(response.Token);
        }

        public static void Login(string userName, string password)
        {
            ComponentContext componentContext = ComponentContextBinder.Lookup();
            var securityService = componentContext.GetComponent<ISecurityService>();
            IAuthenticationTemplate userNamePasswordTemplate = new UserNamePasswordTemplate
            {
                UserName = userName,
                Password = password
            };

            AuthenticationResponse response = securityService.Authenticate(userNamePasswordTemplate);
            SecurityTokenBinder.Bind(response.Token);
        }
    }
}