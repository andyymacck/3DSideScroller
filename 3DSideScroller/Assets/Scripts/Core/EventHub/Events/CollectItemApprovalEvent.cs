using System;

public class CollectItemApprovalEvent : BaseEvent
{
    public Action<CollectableItem> Callback;
    public CollectableItem CollectableItem;

    public CollectItemApprovalEvent(Action<CollectableItem> action, CollectableItem collectableItem)
    {
        Callback = action;
        CollectableItem = collectableItem;
    }       
}
