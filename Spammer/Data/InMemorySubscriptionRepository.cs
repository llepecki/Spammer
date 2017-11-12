using System;
using System.Collections.Generic;
using Spammer.Models;
using System.Linq;

namespace Spammer.Data
{
    internal class InMemorySubscriptionRepository : ISubscriptionRepository
    {
        private static readonly IList<Subscription> _subsctiptions = new List<Subscription>();
        private static readonly IList<Topic> _topics = new List<Topic>();

        static InMemorySubscriptionRepository()
        {
            _topics.Add(new Topic { Abbreviation = "ga", Description = "Guns and Ammo" });
            _topics.Add(new Topic { Abbreviation = "rs", Description = "Rocket Ships" });
            _topics.Add(new Topic { Abbreviation = "hc", Description = "Hover Cars and Lasers" });

            _subsctiptions.Add(new Subscription { Mail = "franek@sztacheta.com", Topics = new List<Topic> { _topics[0] } });
            _subsctiptions.Add(new Subscription { Mail = "zdzisiu@dziaslo.org", Topics = new List<Topic> { _topics[1] } });
            _subsctiptions.Add(new Subscription { Mail = "heniu@kasztan.pl", Topics = new List<Topic> { _topics[0], _topics[1] } });
        }

        public IEnumerable<Topic> GetTopics()
        {
            return _topics;
        }

        public Subscription GetSubscription(string mail)
        {
            return _subsctiptions.SingleOrDefault(subscription => subscription.Equals(mail));
        }

        public IEnumerable<Subscription> GetSubscriptions()
        {
            return _subsctiptions;
        }

        public IEnumerable<Subscription> GetSubscriptions(string abbreviation)
        {
            return _subsctiptions.Where(subscription => subscription.Topics.Any(topic => topic.Equals(abbreviation)));
        }

        public Subscription AddSubscription(Subscription subscription)
        {
            _subsctiptions.Add(subscription);
            return subscription;
        }

        public void RemoveSubscription(Subscription subscriptionToRemove)
        {
            _subsctiptions.Remove(subscriptionToRemove);
        }

        public Subscription ChangeSubscription(Subscription subscriptionToRemove, IEnumerable<string> topicAbbreviations)
        {
            Subscription persistentSubscription = _subsctiptions.SingleOrDefault(subscription => subscription.Equals(subscriptionToRemove));

            if (persistentSubscription != null)
            {
                persistentSubscription.Topics = _topics.Where(topic => topicAbbreviations.Contains(topic.Abbreviation)).ToList();
            }

            return persistentSubscription;
        }
    }
}
