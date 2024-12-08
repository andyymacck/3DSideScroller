namespace LevelManagerLoader
{
    using UnityEngine;
    using System;
    using MgsTools;
    using Object = UnityEngine.Object;
    
    [Serializable]
    public class LevelManagerLevelParam
    {
        public LevelType LevelType;
        public Object Scene;
        public string SceneName;
        public bool Unlocked;
        public int Argument;
        public string Argument_2;
        public Sprite LevelIcon;
        public bool UnlockNextLevelInGroup = true;
        public UnlockedLevelParam[] UnlockNextLevel;
        
        [ReadOnly] public LevelGroupType LevelGroupType;
        [ReadOnly] public string FileName = "Use LM Check";
        [ReadOnly] public int LevelNum;
    }

    [Serializable]
    public struct UnlockedLevelParam
    {
        public LevelGroupType LevelGroupType;
        public int LevelNum;
    }
}