using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.UI.WebAPI.SelfHost
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BasicAuthorizationFilter : ActionFilterAttribute
    {
        private readonly bool _active;
        public BasicAuthorizationFilter(bool active = true)
        {
            _active = active;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!_active)
            {
                return;
            }

            CookieHeaderValue cookie = actionContext.Request.Headers.GetCookies("t123").FirstOrDefault();
            if (cookie == null)
            {
                Challenge(actionContext);
                return;
            }

            SecurityToken token = SecurityToken.Parse(cookie.Cookies[0].Value);
            if (ComponentContextBinder.Lookup().GetComponent<ISecurityService>().VerifyToken(token))
            {
                SecurityTokenBinder.Bind(token);
            }
            else
            {
                Challenge(actionContext);
                return;
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var cookie = new CookieHeaderValue("t123", SecurityTokenBinder.Lookup() == null ? "Invalid" : SecurityTokenBinder.Lookup().ToString())
            {
                Expires = DateTimeOffset.Now.AddDays(1),
                Domain = actionExecutedContext.Request.RequestUri.Host,
                Path = "/"
            };

            actionExecutedContext.Response.Headers.AddCookies(new[] {cookie});

            base.OnActionExecuted(actionExecutedContext);
        }

        private void Challenge(HttpActionContext actionContext)
        {
            string host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }
    }
}