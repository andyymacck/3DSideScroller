namespace LevelManagerLoader
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;
    
    public static class LevelManager
    {
        private static LevelManagerContainer s_container;
        private static LevelManagerLevelParam s_currentLevelParam = new LevelManagerLevelParam();

        public static LevelManagerLevelParam CurrentLevelParam => s_currentLevelParam;
        
        public static void Init()
        {
            if (s_container) return;
            
            s_container = GetContainer();
            if (s_container == null) Debug.LogError("LM Container not find");

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log($"[LEVEL MANAGER] INIT");
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EventHub.Instance.Publish(new LevelManagerOnSceneLoaded(s_currentLevelParam));
        }
        
        private static void InitCheck()
        {
            if (s_container != null) return;
            
            Init();
            if (s_container == null) Debug.LogError("LM Container not find");
        }

        public static LevelManagerContainer GetContainer()
        {
            return Resources.Load("LeveManager", typeof(LevelManagerContainer)) as LevelManagerContainer;
        }

        public static void Restart()
        {
            LoadLevelByNum(s_currentLevelParam.LevelGroupType, s_currentLevelParam.LevelNum);
        }

        public static void LoadLevelByNum(LevelGroupType levelGroupType, int levelNum)
        {
            InitCheck();
            
            LevelManagerLevelParam levelParam = GetLevelManagerParam(levelGroupType, levelNum);
            
            if (levelParam != null)
            {
                LoadLevel(levelParam);
            }
            else
            {
                Debug.LogError($"{LevelManagerConfig.Name}: Level not found - Group:{levelGroupType}, Number:{levelNum}");
            }
        }

        public static void LoadLevelByName(LevelGroupType levelGroupType, string levelName)
        {
            LoadLevelByNum(levelGroupType, GetLevelNumByName(levelGroupType,levelName));
        }
        
        public static void LoadLevelLast()
        {
            InitCheck();

            GetLastLoadLevelParam(out LevelGroupType levelGroupType, out int levelNum);

            if (levelGroupType == LevelGroupType.Empty) levelGroupType = LevelGroupType.Classic;
            
            LoadLevelByNum(levelGroupType, levelNum);
        }

        public static void GetLastLoadLevelParam(out LevelGroupType levelGroupType, out int levelNum)
        {
            InitCheck();

            levelNum = LevelManagerData.LastLevelNum;
            levelGroupType = ConvertStringToEnum(LevelManagerData.LastLevelGroup);
        }
        
        public static bool IsLevelExist(LevelGroupType levelGroupType, int levelNum)
        {
            InitCheck();

            LevelGroup levelGroup = GetLevelGroup(levelGroupType);

            int maxLevel = levelGroup.Levels.Count;
            return levelNum <= maxLevel;
        }
        
        public static bool IsLevelExist(LevelGroup levelGroup, int levelNum)
        {
            InitCheck();

            if (levelNum <= 0)
            {
                Debug.LogError($"{LevelManagerConfig.Name}: Level in group {levelGroup.GroupType} Num can be < 0");
            }
            
            int maxLevel = levelGroup.Levels.Count;
            return levelNum <= maxLevel;
        }

        public static int GetLevelNumByName(LevelGroupType levelGroupType, string levelName)
        {
            InitCheck();
            
            LevelGroup levelGroup = GetLevelGroup(levelGroupType);

            for (int i = 0; i < levelGroup.Levels.Count; i++)
            {
                if(levelGroup.Levels[i].SceneName == levelName)
                {
                    return i + 1;
                }
            }
            
            Debug.LogError($"[Level Manager]: Level not found - LevelGroupType:{levelGroupType}, levelName:{levelName}");
            return 1;
        }

        public static LevelManagerLevelParam GetLevelManagerParam(LevelGroupType levelGroupType, int levelNum)
        {
            //TODO: Rework by TryGetGetLevelManagerParam(out)

            if (levelNum <= 0)
            {
                Debug.LogError($"{LevelManagerConfig.Name}: Level number is < 0");
                return null;
            }
            
            InitCheck();

            LevelGroup levelGroup = GetLevelGroup(levelGroupType);

            if (levelGroup == null)
            {
                Debug.LogError($"{LevelManagerConfig.Name}: Level group is not exist:{levelGroupType}");
                return null;
            }
            
            if (IsLevelExist(levelGroup, levelNum))
            {
                return levelGroup.Levels[levelNum - 1];
            }
            else
            {
                Debug.LogError($"{LevelManagerConfig.Name}: Level is not exist:{levelGroupType}-{levelNum}");
                return null;
            }
        }        
        
        public static bool TryGetNextLevelManagerParam(LevelGroupType levelGroupType, int levelNum, out LevelManagerLevelParam levelParam)
        {
            InitCheck();

            LevelGroup levelGroup = GetLevelGroup(levelGroupType);
            levelParam = null;
            
            if (levelGroup == null) return false;
            
            if (IsLevelExist(levelGroup, levelNum + 1))
            {
                levelParam = levelGroup.Levels[levelNum];
                return true;
            }
            
            return false;
        }

        public static void SetCurrentLevelManagerParamForced(LevelManagerLevelParam levelManagerLevelParam)
        {
            s_currentLevelParam = levelManagerLevelParam;
        }

        public static LevelGroup GetLevelGroup(LevelGroupType levelGroupType)
        {
            InitCheck();
            
            for (int i = 0; i < s_container.LevelGroups.Count; i++)
            {
                if (s_container.LevelGroups[i].GroupType == levelGroupType)
                    return s_container.LevelGroups[i];
            }

            return null;
        }

        private static Object GetLevelObjectInGroupByNum(LevelGroupType levelGroupType, int levelNum)
        {
            LevelManagerLevelParam levelParam = GetLevelManagerParam(levelGroupType, levelNum);

            return levelParam?.Scene;
        }

        private static Object GetLevelObjectInGroupByName(LevelGroupType levelGroupType, string levelName)
        {
            LevelGroup levelGroup = GetLevelGroup(levelGroupType);

            if (levelGroup == null) return null;

            for (int i = 0; i < levelGroup.Levels.Count; i++)
            {
                if (levelGroup.Levels[i].SceneName == levelName)
                    return levelGroup.Levels[i].Scene;
            }

            return null;
        }

        private static void LoadLevel(LevelManagerLevelParam levelParam)
        {
            InitCheck();

            s_currentLevelParam = levelParam;
            LevelManagerData.LastLevelNum = levelParam.LevelNum;
            LevelManagerData.LastLevelGroup = levelParam.LevelGroupType.ToString();
            SceneManager.LoadScene(levelParam.FileName);
            Debug.Log($"[LEVEL MANAGER] Load Level - Group:{levelParam.LevelGroupType}, Number:{levelParam.LevelNum}");
        }

        private static LevelGroupType ConvertStringToEnum(string name)
        {
            bool success = Enum.TryParse(name, out LevelGroupType levelGroup);

            if(success)
            {
                return levelGroup;
            }
            else
            {
                return LevelGroupType.Empty;
                throw new InvalidOperationException($"Cannot convert {name} to LevelGroups enum");
            }
        }
    }
}