using System;
using System.Collections.Generic;

namespace MintPlayer.MVVM.Platforms.Common.Events
{
    public interface IAggregatorEvent
    {
        void Subscribe(Action<object> handler);
        void Unsubscribe(Action<object> handler);
        void Publish(object parameter);
    }
    internal class AggregatorEvent<TEvent> : IAggregatorEvent
    {
        private readonly IList<Action<object>> handlers;
        public AggregatorEvent()
        {
            handlers = new List<Action<object>>();
        }

        public void Subscribe(Action<object> handler)
        {
            handlers.Add(handler);
        }

        public void Unsubscribe(Action<object> handler)
        {
            handlers.Remove(handler);
        }

        public void Publish(object parameter)
        {
            foreach (var handler in handlers)
            {
                handler(parameter);
            }
        }
    }
}
