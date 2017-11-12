using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text;

namespace Spammer.Security
{
    public class TrivialAuthenticationHandler : AuthenticationHandler<TrivialAuthenticationOptions>
    {
        public TrivialAuthenticationHandler(IOptionsMonitor<TrivialAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            string basicPrefix = "Basic";

            if (!authorizationHeader.StartsWith(basicPrefix))
            {
                AuthenticateResult.NoResult();
            }

            string encodedCredentials = authorizationHeader.Substring(basicPrefix.Length).Trim();

            if (string.IsNullOrEmpty(encodedCredentials))
            {
                return AuthenticateResult.Fail("No credentials");
            }

            string decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            string[] userAndPassword = decodedCredentials.Split(':');

            var user = userAndPassword[0];
            var password = userAndPassword[1];

            var context = new ValidateUserAndPasswordContext(Context, Scheme, Options)
            {
                User = user,
                Password = password
            };

            await Options.Validate(context);

            if (context.Result != null)
            {
                var ticket = new AuthenticationTicket(context.Principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.NoResult();
        }
    }
}
