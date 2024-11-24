using System;
using System.Collections.Generic;


public class EventHub
{
    private static EventHub s_instance;
    public static EventHub Instance 
    { 
        get 
        { 
            if (s_instance == null)
            {
                s_instance = new EventHub();
            }
                
            return s_instance; 
        } 
    }

    private readonly Dictionary<Type, List<Delegate>> m_subscribers = new Dictionary<Type, List<Delegate>>();


    public void Subscribe<T>(Action<T> callbackAction) where T : BaseEvent
    {
        var eventType = typeof(T);

        if (!m_subscribers.ContainsKey(eventType))
        {
            m_subscribers[eventType] = new List<Delegate>();
        }

        m_subscribers[eventType].Add(callbackAction);
    }

    public void UnSubscribe<T>(Action<T> callbackAction) where T : BaseEvent
    {
        var eventType = typeof(T);

        if (m_subscribers.ContainsKey(eventType))
        {
            m_subscribers[eventType].Remove(callbackAction);
        }
    }

    public void Publish<T>(T eventData) where T : BaseEvent
    {
        var eventType = typeof(T);

        if (m_subscribers.ContainsKey(eventType))
        {
            foreach (Action<T> subscriber in m_subscribers[eventType])
            {
                if (subscriber != null)
                {
                    subscriber.Invoke(eventData);
                }
            }
        }
    }
}