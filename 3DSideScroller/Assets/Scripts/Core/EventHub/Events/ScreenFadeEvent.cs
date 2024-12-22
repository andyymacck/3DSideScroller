
public class ScreenFadeEvent : BaseEvent
{
    public bool IsFadeOut  { get; }

    public ScreenFadeEvent(bool isFadeOut)
    {
        IsFadeOut = isFadeOut;
    }
}