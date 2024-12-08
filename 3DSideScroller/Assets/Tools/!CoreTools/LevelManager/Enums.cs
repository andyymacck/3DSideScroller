namespace LevelManagerLoader
{
    public enum LevelGroupType 
    {
        Empty = 0, 
        Menu = 1, 
        Classic = 2,
        Time = 3,
        Thief = 4,
    };

    public enum GroupGameParam
    {
        Empty = 0, 
        Warning = 1
    };
    
    public enum LevelGameParam
    {
        Open,
        Complete,
        Main,
        Timer,
        Coin,
        Gold
    };

    public enum LevelType
    {
        Empty,
        Menu,
        HiddenObject,
        Diorama
    }
}
