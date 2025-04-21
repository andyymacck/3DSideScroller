using System.Collections.Generic;
using System;
using UnityEngine;

public class ServiceProvider
{
    private readonly Dictionary<Type, IService> m_services = new Dictionary<Type, IService>();
    private static ServiceProvider s_instance;

    public static void Initialize()
    {
        s_instance = new ServiceProvider();
    }

    public static void Register<T>(T service) where T : IService
    {
        Type type = typeof(T);

        if(!s_instance.m_services.ContainsKey(type))
        {
            s_instance.m_services.Add(type, service);
        }
        else
        {
            Debug.LogError($"{type} is already added");
        } 
    }

    public static void Unegister<T>(T service) where T : IService
    {
        Type type = typeof(T);

        if (s_instance.m_services.ContainsKey(type))
        {
            s_instance.m_services.Remove(type);
        }
        else
        {
            Debug.LogError($"{type} is already removed");
        }
    }

    public static T GetService<T>() where T : IService
    {
        Type type = typeof(T);

        if (s_instance.m_services.ContainsKey(type)) 
        {
            return (T)s_instance.m_services[type];
        }
        else
        {
            Debug.LogError($"{type} not found");
            return (T) default;
        }
    }
}