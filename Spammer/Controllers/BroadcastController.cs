using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Spammer.Data;
using Spammer.Sending;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Spammer.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/broadcast")]
    public class BroadcastController : Controller
    {
        private readonly ISubscriptionRepository _repository;
        private readonly ISendContent _sendContent;

        public BroadcastController(ISubscriptionRepository repository, ISendContent sendContent)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (sendContent == null)
            {
                throw new ArgumentNullException(nameof(sendContent));
            }

            _repository = repository;
            _sendContent = sendContent;
        }

        [HttpPost]
        [Route("{abbreviation:topic}")]
        public IActionResult Send(string abbreviation, [FromBody] Content content)
        {
            IEnumerable<string> mails = _repository.GetSubscriptions(abbreviation)
                .Select(subscription => subscription.Mail);

            _sendContent.Send(mails, content);

            return Accepted();
        }
    }
}