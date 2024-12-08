namespace LevelManagerLoader
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;
    
    /*
    public enum LevelGroupType { Empty, Menu, Levels };
    public enum LevelGameParam { Open, Complete, Main, Thief };
    
    public static class LevelManagerCore
    {
        private static LevelManagerContainer s_container;
        private static LevelManagerLevelParam s_currentLevelParam;
        private static string s_currentLevelGroup;
        private static int s_currentLevelNum;

        public static LevelManagerLevelParam CurrentLevelParam => s_currentLevelParam;
        public static string CurrentLevelGroup => s_currentLevelGroup;
        public static int CurrentLevelNum => s_currentLevelNum;

        public static void Init()
        {
            s_container = GetContainer();
        }
        
        private static void InitCheck()
        {
            if (!s_container) Init();

            if (!LevelManagerData.HasLevelData(LevelGroupType.Levels, 1))
            {
                LevelManagerData.SetLevelOpen(LevelGroupType.Levels, 1, true);
            }
        }

        public static LevelManagerContainer GetContainer()
        {
            return Resources.Load("LeveManager", typeof(LevelManagerContainer)) as LevelManagerContainer;
        }

        public static void Restart()
        {
            
        }

        public static void LoadLevelByNum(LevelGroupType levelGroupType, int levelNum)
        {
            InitCheck();
            
            LevelManagerLevelParam levelParam = GetLevelParam(levelGroupType, levelNum);
            
            
            //Debug.Log("LOAD LoadLevelByNum " + levelGroupType + " " + levelNum);
            //Debug.Log("LOAD Scene " + level.name);

            if (levelParam != null || levelParam.Scene != null)
            {
                LoadLevel(levelParam, levelGroupType, levelNum);
            }
        }

        public static void LoadLevelLast()
        {
            GetLastLoadLevelParam(out LevelGroupType levelGroupType, out int levelNum);

            if (levelGroupType == LevelGroupType.Empty) levelGroupType = LevelGroupType.Levels;
            
            LoadLevelByNum(levelGroupType, levelNum);
        }

        public static void GetLastLoadLevelParam(out LevelGroupType levelGroupType, out int levelNum)
        {
            levelNum = LevelManagerSave.LastLevelNum;
            levelGroupType = ConvertStringToEnum(LevelManagerSave.LastLevelGroup);
        }

        public static bool IsLevelExist(LevelGroupType levelGroupType, int levelNum)
        {
            LevelGroup levelGroup = GetLevelGroup(levelGroupType);

            int maxLevel = levelGroup.Levels.Count;
            return levelNum <= maxLevel;
        }
        
        public static bool IsLevelExist(LevelGroup levelGroup, int levelNum)
        {
            if (levelNum == 0)
            {
                Debug.LogError("Level Num can be < 1");
            }
            
            int maxLevel = levelGroup.Levels.Count;
            return levelNum <= maxLevel;
        }

        public static LevelManagerLevelParam GetLevelParam(LevelGroupType levelGroupType, int levelNum)
        {
            LevelGroup levelGroup = GetLevelGroup(levelGroupType);

            if (levelGroup == null) return null;

            if (IsLevelExist(levelGroup, levelNum))
            {
                return levelGroup.Levels[levelNum - 1];
            }
            else
            {
                return null;
            }
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
            LevelManagerLevelParam levelParam = GetLevelParam(levelGroupType, levelNum);

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

        private static void LoadLevel(LevelManagerLevelParam levelParam, LevelGroupType levelGroupType, int levelNum)
        {
            s_currentLevelParam = levelParam;
            s_currentLevelNum = levelNum;
            s_currentLevelGroup = levelParam.ToString();
            LevelManagerSave.LastLevelNum = levelNum;
            LevelManagerSave.LastLevelGroup = levelParam.ToString();
            SceneManager.LoadScene(levelParam.Scene.name);
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
    */
}