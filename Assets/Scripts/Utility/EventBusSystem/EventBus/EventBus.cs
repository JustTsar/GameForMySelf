using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.EventBusSystem.Interfaces;

namespace Utility.EventBusSystem.EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> handlersByType = new Dictionary<Type, List<Delegate>>();

        public static ISubscription Subscribe<T>(Action handler) where T : IEvent
        {
            return Subscribe<T>(args => handler());
        }
        
        public static ISubscription Subscribe<T>(Action<T> handler) where T : IEvent
        {
            Type type = typeof(T);
            if (!handlersByType.TryGetValue(type, out var handlers))
            {
                handlers = new List<Delegate>();
                handlersByType.Add(type, handlers);
            }

            handlers.Add(handler);
            return new EventBusSubscription<T>(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            Type type = typeof(T);
            if (handlersByType.TryGetValue(type, out var handlers))
            {
                handlers.Remove(handler);
            }
        }

        public static void Dispatch<T>(T @event) where T : IEvent
        {
            var type = @event.GetType();
            if (handlersByType.TryGetValue(type, out var handlers))
            {
                foreach (var handler in handlers.ToArray())
                {
                    try
                    {
                        handler.DynamicInvoke(@event);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
    }
}