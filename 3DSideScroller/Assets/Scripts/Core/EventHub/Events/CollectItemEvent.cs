using System.Collections.Generic;

public class CollectItemEvent : BaseEvent
{
    public Dictionary<CollectabeType, CollectableModel> Models { get; }

    public CollectItemEvent(Dictionary<CollectabeType, CollectableModel> models)
    {
        Models = models;
    }
}