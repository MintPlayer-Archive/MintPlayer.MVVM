using System;
using System.Collections.Generic;

namespace MintPlayer.MVVM.Platforms.Common.Events
{
    public interface IEventAggregator
    {
        IAggregatorEvent GetEvent<TEvent>() where TEvent : IPubSubEvent, new();
    }

    internal class EventAggregator : IEventAggregator
    {
        private readonly IDictionary<Type, IAggregatorEvent> registeredEvents;
        public EventAggregator()
        {
            registeredEvents = new Dictionary<Type, IAggregatorEvent>();
        }

        public IAggregatorEvent GetEvent<TEvent>() where TEvent : IPubSubEvent, new()
        {
            var eventType = typeof(TEvent);
            if (registeredEvents.ContainsKey(eventType))
            {
                return registeredEvents[eventType];
            }
            else
            {
                var ev = new AggregatorEvent<TEvent>();
                registeredEvents.Add(eventType, ev);
                return ev;
            }
        }
    }
}
