using LevelManagerLoader;

public class LevelManagerOnSceneLoaded : BaseEvent
{
    public LevelManagerLevelParam levelManagerLevelParam;

    public LevelManagerOnSceneLoaded (LevelManagerLevelParam levelManagerLevelParam)
    {
        this.levelManagerLevelParam = levelManagerLevelParam;
    }
}
