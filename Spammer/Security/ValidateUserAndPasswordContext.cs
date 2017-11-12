using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Spammer.Security
{
    public class ValidateUserAndPasswordContext : ResultContext<TrivialAuthenticationOptions>
    {
        public ValidateUserAndPasswordContext(
            HttpContext context,
            AuthenticationScheme scheme,
            TrivialAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public string User { get; set; }

        public string Password { get; set; }
    }
}