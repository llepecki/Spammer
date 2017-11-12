using Spammer.Models;
using System.Collections.Generic;

namespace Spammer.Data
{
    public interface ISubscriptionRepository
    {
        IEnumerable<Topic> GetTopics();

        IEnumerable<Subscription> GetSubscriptions();

        IEnumerable<Subscription> GetSubscriptions(string topicAbbreviation);

        Subscription GetSubscription(string mail);

        Subscription AddSubscription(Subscription subscription);

        Subscription ChangeSubscription(Subscription subscription, IEnumerable<string> topicAbbreviations);

        void RemoveSubscription(Subscription subscription);
    }
}