using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Compositional.Composer;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Infrastructure.Web;
using MeshkatEnterprise.Infrastructure.Web.Security;
using MeshkatEnterprise.Security.API;
using Newtonsoft.Json;

namespace MeshkatEnterprise.Booklet.UI.MVC.Areas.Security.Controllers
{
    public class AccountController : Controller
    {
        [MeshkatEnterprise.Security.API.AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [MeshkatEnterprise.Security.API.AllowAnonymous]
        public string LogIn(string userName, string password)
        {
            ComponentContext componentContext = ComponentContextBinder.Lookup();
            var securityService = componentContext.GetComponent<ISecurityService>();
            IAuthenticationTemplate userNamePasswordTemplate = new UserNamePasswordTemplate
            {
                UserName = userName,
                Password = password
            };

            string msg = "";
            AuthenticationResponse response = securityService.Authenticate(userNamePasswordTemplate);

            if (response.Successful)
            {
                SecurityHelper.SetTokenOnClient(Response, response.Token);
            }
            else
            {
                msg = ResourceGlobalizer.GetText(response.Message);
            }

            var k = new KeyValuePair<bool, string>(response.Successful, msg);

            return JsonConvert.SerializeObject(k);
        }


        [HttpGet]
        [MeshkatEnterprise.Security.API.AllowAnonymous]
        public ActionResult LogOut()
        {
            SecurityHelper.RemoveCookie(Response);
            SecurityTokenBinder.Unbind();
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [MeshkatEnterprise.Security.API.AllowAnonymous]
        public string TokenVerificationFailed()
        {
            SecurityHelper.RemoveCookie(Response);
            SecurityTokenBinder.Unbind();
            return "TokenVerificationFailed";
        }
    }
}