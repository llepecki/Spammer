using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Spammer.Security
{
    public class TrivialAuthenticationOptions : AuthenticationSchemeOptions
    {
        public Task Validate(ValidateUserAndPasswordContext context)
        {
            if (context.User == User && context.Password == Password)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, context.User, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, context.User, ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();
            }

            return Task.CompletedTask;
        }

        public string User { private get; set; }

        public string Password { private get; set; }
    }
}