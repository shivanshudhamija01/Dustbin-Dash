using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceContainer
{
    private static Dictionary<Type, object> container = new Dictionary<Type, object>();
    public static void MapService<T>(T service)
    {
        container[typeof(T)] = service;
    }
    public static T Get<T>()
    {
        return (T)container[typeof(T)];
    }
}
