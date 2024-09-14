using System;

public abstract class BaseEvent
{
    public DateTime EventTime { get; }

    protected BaseEvent()
    {
        EventTime = DateTime.Now;
    }
}