using System;
using Utility.EventBusSystem.Interfaces;

namespace Utility.EventBusSystem.EventBus
{
    public class EventBusSubscription<T> : ISubscription where T : IEvent
    {
        private readonly Action<T> handler;

        public EventBusSubscription(Action<T> handler)
        {
            this.handler = handler;
        }

        public void Unsubscribe()
        {
            EventBus.Unsubscribe(handler);
        }
    }
}