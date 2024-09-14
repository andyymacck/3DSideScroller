public class IsGroundedEvent : BaseEvent
{
    public bool IsGrounded { get; }

    public IsGroundedEvent(bool isGrounded)
    {
        IsGrounded = isGrounded;
    }
}