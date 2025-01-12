using UnityEngine;

namespace SideScroller
{
    public class TeleportEvent : BaseEvent
    {
        public Vector3 Destination { get; }

        public TeleportEvent(Vector3 destination)
        {
            Destination = destination;
        }
    }
}