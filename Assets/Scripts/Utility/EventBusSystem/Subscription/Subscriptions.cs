using System.Collections.Generic;
using Utility.EventBusSystem.Interfaces;

namespace Utility.EventBusSystem.Subscription
{
    public class Subscriptions : ISubscription
    {
        private readonly List<ISubscription> subscriptions;

        public Subscriptions()
        {
            subscriptions = new List<ISubscription>();
        }

        public Subscriptions Add(ISubscription subscription)
        {
            this.subscriptions.Add(subscription);
            return this;
        }

        public Subscriptions Add(IEnumerable<ISubscription> subscriptions)
        {
            this.subscriptions.AddRange(subscriptions);
            return this;
        }

        public void UnsubscribeAll()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Unsubscribe();
            }

            subscriptions.Clear();
        }

        void ISubscription.Unsubscribe()
        {
            UnsubscribeAll();
        }
    }
}