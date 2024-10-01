using System;
using _Game.Scripts.Utility.EventBusSystem.Interfaces;

namespace _Game.Scripts.Utility.EventBusSystem.EventBus
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
            global::Utility.EventBusSystem.EventBus.EventBus.Unsubscribe(handler);
        }
    }
}