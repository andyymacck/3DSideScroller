using System;

public class CollectItemApprovalEvent : BaseEvent
{
    private CollectableItem m_collectableItem;
    private Action<CollectableItem> m_callback;
    private bool m_isApproved = false;

    public CollectableItem CollectableItem => m_collectableItem;

    public CollectItemApprovalEvent(Action<CollectableItem> action, CollectableItem collectableItem)
    {
        m_callback = action;
        m_collectableItem = collectableItem;
    }

    public void Approve()
    {
        if (!m_isApproved)
        {
            m_callback(m_collectableItem);
            m_isApproved = true;
        }
    }
}