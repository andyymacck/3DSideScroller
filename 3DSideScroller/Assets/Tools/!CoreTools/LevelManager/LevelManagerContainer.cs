namespace LevelManagerLoader
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using Object = UnityEngine.Object;
    
    [CreateAssetMenu(fileName ="LeveManager",menuName = "Level Manager Levels Container")]
    public class LevelManagerContainer : ScriptableObject
    {
        [SerializeField] public Object StartGameScene;
        [SerializeField] public Object LoadingScene;
        [SerializeField] public List<LevelGroup> LevelGroups = new List<LevelGroup>();
        
        public string LoadingSceneFileName;
    }

    [Serializable]
    public class LevelGroup
    {
        public LevelGroupType GroupType;
        public List<LevelManagerLevelParam> Levels = new List<LevelManagerLevelParam>();
    }
}