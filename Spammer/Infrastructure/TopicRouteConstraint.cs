using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Spammer.Infrastructure
{
    public class TopicRouteConstraint : IRouteConstraint
    {
        public const string Name = "topic";

        private static readonly Regex _topicRegex = new Regex(@"^[a-zA-Z]{2}$");

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            try
            {
                string topic = values[routeKey].ToString();
                return _topicRegex.IsMatch(topic);
            }
            catch
            {
                return false;
            }
        }
    }
}