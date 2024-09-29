using System;
using Utility.EventBusSystem.Interfaces;

namespace Utility.EventBusSystem.Subscription
{
    public class Subscription : ISubscription
    {
        private readonly Action unsubscribe;

        public Subscription(Action unsubscribe)
        {
            this.unsubscribe = unsubscribe;
        }

        public void Unsubscribe()
        {
            unsubscribe();
        }
    }
}