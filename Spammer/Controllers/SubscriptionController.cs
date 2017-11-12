using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Spammer.Data;
using System;
using Spammer.Models;
using System.Linq;
using System.Collections.Generic;
using Spammer.Sending;

namespace Spammer.Controllers
{
    [Produces("application/json")]
    [Route("api/subscriptions")]
    public class SubscriptionController : Controller
    {
        const string GetSubscriptionRouteName = "GetSubscriptionRoute";

        private static Content UnsubscribeConfirmation => new Content
            {
                Subject = "You have unsubscribed from all the topics",
                Message = "Hope to see you back some day",
            };

        private readonly ISubscriptionRepository _repository;
        private readonly ISendContent _sendContent;

        public SubscriptionController(ISubscriptionRepository repository, ISendContent sendContent)
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

        [Route("{address:mail}", Name = GetSubscriptionRouteName)]
        [HttpGet()]
        public Subscription GetSubscription(string address)
        {
            Subscription subscription = _repository.GetSubscription(address);

            if (subscription == null)
            {
                subscription = new Subscription { Mail = address, Topics = new List<Topic>() };
            }

            return subscription;
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<Subscription> GetSubscriptions()
        {
            return _repository.GetSubscriptions();
        }

        [Route("{address:mail}")]
        [HttpPut]
        public IActionResult ChangeSubscription(string address, [FromBody] string[] topicAbbreviations)
        {
            Subscription subscription = _repository.GetSubscription(address);

            if (topicAbbreviations.Length == 0)
            {
                if (subscription != null)
                {
                    RemoveSubscriptionAndSendConfirmation(subscription);
                }

                return Accepted();
            }
            else
            {
                if (subscription == null)
                {
                    subscription = _repository.AddSubscription(new Subscription { Mail = address });
                }

                subscription = _repository.ChangeSubscription(subscription, topicAbbreviations);

                return CreatedAtRoute(GetSubscriptionRouteName, subscription);
            }
        }

        [Route("{address:mail}")]
        [HttpDelete]
        public IActionResult Unsubscribe(string address)
        {
            Subscription subscription = _repository.GetSubscription(address);

            if (subscription != null)
            {
                RemoveSubscriptionAndSendConfirmation(subscription);
            }

            return Accepted();
        }

        private void RemoveSubscriptionAndSendConfirmation(Subscription subscription)
        {
            _repository.RemoveSubscription(subscription);
            _sendContent.Send(subscription.Mail, UnsubscribeConfirmation);
        }
    }
}