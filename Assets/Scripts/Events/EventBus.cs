using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    private readonly Dictionary<Type, Delegate> _handlers = new();
    private readonly Dictionary<Type, IStickyWrapper> _stickyEvents = new();

    public void Subscribe<T>(Action<T> handler, bool receiveSticky = true) where T : struct
    {
        if (_handlers.TryGetValue(typeof(T), out var existing))
            _handlers[typeof(T)] = Delegate.Combine(existing, handler);
        else
            _handlers[typeof(T)] = handler;

        if (receiveSticky && _stickyEvents.TryGetValue(typeof(T), out var sticky))
            sticky.ReplayTo(handler);
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
            ((Action<T>)callback)(evt);
    }

    public void PublishSticky<T>(T evt) where T : struct
    {
        _stickyEvents[typeof(T)] = new StickyWrapper<T>(evt);
        Publish(evt);
    }

    public void ClearSticky<T>() where T : struct
    {
        _stickyEvents.Remove(typeof(T));
    }

    public void ClearAll()
    {
        _handlers.Clear();
        _stickyEvents.Clear();
    }
}

public interface IStickyWrapper
{
    void ReplayTo(Delegate handler);
}

public class StickyWrapper<T> : IStickyWrapper where T : struct
{
    private readonly T _value;

    public StickyWrapper(T value)
    {
        _value = value;
    }

    public void ReplayTo(Delegate handler)
    {
        if (handler is Action<T> typed)
            typed(_value);
    }
}