using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;

namespace Spammer.Infrastructure
{
    public class MailRouteConstraint : IRouteConstraint
    {
        public const string Name = "mail";

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            try
            {
                new MailAddress(values[routeKey].ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
