using System;
using System.Collections.Generic;

public class EventBus
{
    private readonly Dictionary<Type, Delegate> _handlers = new();

    public void Subscribe<T>(Action<T> handler) where T : struct
    {
        if (_handlers.TryGetValue(typeof(T), out var existing))
            _handlers[typeof(T)] = Delegate.Combine(existing, handler);
        else
            _handlers[typeof(T)] = handler;
    }

    public void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        if (_handlers.TryGetValue(typeof(T), out var existing))
        {
            var current = Delegate.Remove(existing, handler);
            if (current == null)
                _handlers.Remove(typeof(T));
            else
                _handlers[typeof(T)] = current;
        }
    }

    public void Publish<T>(T evt) where T : struct
    {
        if (_handlers.TryGetValue(typeof(T), out var callback))
        {
            ((Action<T>)callback)(evt); // no boxing
        }
    }

    public void ClearAll()
    {
        _handlers.Clear();
    }
}
