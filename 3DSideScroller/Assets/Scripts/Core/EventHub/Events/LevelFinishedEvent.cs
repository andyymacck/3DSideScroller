public class LevelFinishedEvent : BaseEvent
{
    private int m_score;

    public LevelFinishedEvent(int score)
    {
        m_score = score;
    }
}