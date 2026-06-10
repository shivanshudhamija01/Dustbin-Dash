using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> Events = new();

    public static void Subscribe<T>(Action<T> listener)
    {
        if (Events.TryGetValue(typeof(T), out var existing))
            Events[typeof(T)] = Delegate.Combine(existing, listener);
        else
            Events[typeof(T)] = listener;
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        if (!Events.TryGetValue(typeof(T), out var existing))
            return;

        var current = Delegate.Remove(existing, listener);

        if (current == null)
            Events.Remove(typeof(T));
        else
            Events[typeof(T)] = current;
    }

    public static void Publish<T>(T eventData)
    {
        if (Events.TryGetValue(typeof(T), out var existing))
            ((Action<T>)existing)?.Invoke(eventData);
    }
}