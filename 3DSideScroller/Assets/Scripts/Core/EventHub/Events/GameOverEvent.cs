public class GameOverEvent : BaseEvent
{
    private int m_score;

    public GameOverEvent(int score)
    {
        m_score = score;
    }
}