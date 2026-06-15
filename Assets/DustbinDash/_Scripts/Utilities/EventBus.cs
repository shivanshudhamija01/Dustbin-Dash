using System;
using System.Collections.Generic;

public class EventBus : IEventBus
{
    // private static readonly Dictionary<Type, Delegate> Events = new();

    // public static void Subscribe<T>(Action<T> listener)
    // {
    //     if (Events.TryGetValue(typeof(T), out var existing))
    //         Events[typeof(T)] = Delegate.Combine(existing, listener);
    //     else
    //         Events[typeof(T)] = listener;
    // }

    // public static void Unsubscribe<T>(Action<T> listener)
    // {
    //     if (!Events.TryGetValue(typeof(T), out var existing))
    //         return;

    //     var current = Delegate.Remove(existing, listener);

    //     if (current == null)
    //         Events.Remove(typeof(T));
    //     else
    //         Events[typeof(T)] = current;
    // }

    // public static void Publish<T>(T eventData)
    // {
    //     if (Events.TryGetValue(typeof(T), out var existing))
    //         ((Action<T>)existing)?.Invoke(eventData);
    // }
    private readonly Dictionary<Type, Delegate> events = new();
    public void Subscribe<T>(Action<T> listener)
    {
        var type = typeof(T);
        if (!events.ContainsKey(type)) events[type] = null;
        events[type] = Delegate.Combine(events[type], listener);
    }
    public void Unsubscribe<T>(Action<T> listener)
    {
        var type = typeof(T);
        if (events.TryGetValue(type, out var listeners))
        {
            var result = Delegate.Remove(listeners, listener);
            if (result == null)
            {
                events.Remove(type);
            }
            else
            {
                events[type] = result;
            }
        }
    }
    public void Publish<T>(T gameEvent)
    {
        var type = typeof(T);
        if (events.TryGetValue(type, out var listener))
        {
            var callbackmethods = listener as Action<T>;
            callbackmethods?.Invoke(gameEvent);
        }
    }



}