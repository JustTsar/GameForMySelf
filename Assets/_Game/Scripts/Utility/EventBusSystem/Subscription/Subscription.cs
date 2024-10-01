using System;
using _Game.Scripts.Utility.EventBusSystem.Interfaces;

namespace _Game.Scripts.Utility.EventBusSystem.Subscription
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