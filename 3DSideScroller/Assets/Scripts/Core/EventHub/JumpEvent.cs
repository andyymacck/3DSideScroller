public class JumpEvent : BaseEvent
{
    public int JumpsCount { get; }

    public JumpEvent(int jumpsCount)
    {
        JumpsCount = jumpsCount;
    }
}