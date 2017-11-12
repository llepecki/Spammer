using System;
using Microsoft.AspNetCore.Mvc;
using Spammer.Data;
using Spammer.Models;
using System.Collections.Generic;

namespace Spammer.Controllers
{
    [Produces("application/json")]
    [Route("api/topics")]
    public class TopicController : Controller
    {
        private readonly ISubscriptionRepository _repository;

        public TopicController(ISubscriptionRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Topic> GetSubscriptions()
        {
            return _repository.GetTopics();
        }
    }
}