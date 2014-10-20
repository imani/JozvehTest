using System;
using System.Text;
using System.Web.Http;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.UI.WebAPI.SelfHost
{
    [BasicAuthorizationFilter(false)]
    public class AccountController : ApiController
    {
        [HttpGet]
        public string LogIn(string authInfo)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authInfo)).Split(new[] {':'});
            
            IAuthenticationTemplate userNamePasswordTemplate = new UserNamePasswordTemplate
            {
                UserName = credentials[0],
                Password = credentials[1]
            };

            AuthenticationResponse authenticationResponse =
                ComponentContextBinder.Lookup().GetComponent<ISecurityService>().Authenticate(userNamePasswordTemplate);

            if (authenticationResponse.Successful)
            {
                SecurityTokenBinder.Bind(authenticationResponse.Token);
            }

            return authenticationResponse.Message;//if success return null else return message in xml format
        }

        [HttpGet]
        public void LogOut()
        {
            SecurityTokenBinder.Unbind();
        }
    }
}