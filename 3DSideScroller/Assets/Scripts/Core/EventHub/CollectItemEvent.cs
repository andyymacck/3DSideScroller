using System.Collections.Generic;

public class CollectItemEvent : BaseEvent
{
    public GemModel GemModel { get; }

    public CollectItemEvent(GemModel gem)
    {
        GemModel = gem;
    }
}
