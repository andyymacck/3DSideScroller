using UnityEngine;

namespace LevelManagerLoader
{
   using MgsTools.Data;
   
   public class LevelManagerData : LevelManagerDataCore
   {
      private static string id = "lm_";
      
      public static int LastLevelNum
      {
         get => SavedData.GetInt(id + "last_level_num", 0);
         set => SavedData.SetInt(id + "last_level_num", value);
      }
        
      public static string LastLevelGroup
      {
         get => SavedData.GetString(id + "last_level_name", "");
         set => SavedData.SetString(id + "last_level_name", value);
      }      
      
      public static void SetMaxOpenLevel(LevelGroupType levelGroupType, int levelNum)
      {
         SavedData.SetInt(id + "last_" + levelGroupType, levelNum);
      }

      public static void SetGroupData(LevelGroupType levelGroupType, string key, string value)
      {
         SavedData.StringArrayWithKeyData.SetValueByKey(id + "group_data_" + levelGroupType, key, value);
      }
      
      public static string GetGroupData(LevelGroupType levelGroupType, string key)
      {
         return SavedData.StringArrayWithKeyData.GetValueByKey(id + "group_data_" + levelGroupType, key);
      }
      
      public static void RemoveGroupDataKeyValue(LevelGroupType levelGroupType, string key)
      {
         SavedData.StringArrayWithKeyData.RemoveKeyValue(id + "group_data_" + levelGroupType, key);
      }
      
      public static void SetLevelDataMain(LevelGroupType levelGroupType, int levelNum, int totalItems, int collectedItems)
      {
         string param = totalItems + c_splitChar.ToString() + collectedItems;
         SetLevelData(levelGroupType, levelNum, LevelGameParam.Main, param);
      }

      public static void GetLevelDataMain(LevelGroupType levelGroupType, int levelNum, out int totalItems, out int collectedItems)
      {
         string[] levelData = GetLevelData(levelGroupType, levelNum, LevelGameParam.Main);

         if (IsEmpty(levelData))
         {
            collectedItems = 0;
            totalItems = 0;
         }
         else
         {
            collectedItems = int.Parse(levelData[1]);
            totalItems = int.Parse(levelData[0]);
         }
      }      
      
      public static void SetLevelDataCoin(LevelGroupType levelGroupType, int levelNum, int totalCoins, int collectedCoins)
      {
         string param = totalCoins + c_splitChar.ToString() + collectedCoins;
         SetLevelData(levelGroupType, levelNum, LevelGameParam.Coin, param);
      }

      public static void GetLevelDataCoin(LevelGroupType levelGroupType, int levelNum, out int totalCoins, out int collectedCoins)
      {
         string[] levelData = GetLevelData(levelGroupType, levelNum, LevelGameParam.Coin);

         if (IsEmpty(levelData))
         {
            collectedCoins = 0;
            totalCoins = 0;
         }
         else
         {
            collectedCoins = int.Parse(levelData[1]);
            totalCoins = int.Parse(levelData[0]);
         }
      }
      
      public static void SetLevelDataGold(LevelGroupType levelGroupType, int levelNum, int totalGolds, int collectedGolds)
      {
         string param = totalGolds + c_splitChar.ToString() + collectedGolds;
         SetLevelData(levelGroupType, levelNum, LevelGameParam.Gold, param);
      }

      public static void GetLevelDataGold(LevelGroupType levelGroupType, int levelNum, out int totalGolds, out int collectedGolds)
      {
         string[] levelData = GetLevelData(levelGroupType, levelNum, LevelGameParam.Gold);

         if (IsEmpty(levelData))
         {
            collectedGolds = 0;
            totalGolds = 0;
         }
         else
         {
            collectedGolds = int.Parse(levelData[1]);
            totalGolds = int.Parse(levelData[0]);
         }
      }
      //
      
      public static bool HasLevelData(LevelGroupType levelGroupType, int levelNum, LevelGameParam gameParam)
      {
         return HasValueByKey(levelGroupType, levelNum, gameParam.ToString());
      }
      
      /*public static bool HasLevelData(LevelGroupType levelGroupType, int levelNum)
      {
         string[] rawData = ReadRawLevelParams(levelGroupType, levelNum);

         return !IsEmpty(rawData);
      }*/

      public static void SetLevelOpen(LevelGroupType levelGroupType, int levelNum, bool isOpen)
      {
         int param = isOpen ? 1 : 0;
         SetLevelData(levelGroupType, levelNum, LevelGameParam.Open, param.ToString());
      }

      public static bool GetLevelUnlocked(LevelGroupType levelGroupType, int levelNum)
      {
         LevelManagerLevelParam levelParam = LevelManager.GetLevelManagerParam(levelGroupType, levelNum);

         if (levelParam == null) return false;
         if (LevelManager.GetLevelManagerParam(levelGroupType, levelNum).Unlocked) return true;
         if (!HasLevelData(levelGroupType, levelNum, LevelGameParam.Open)) return false;

         string levelData = GetLevelData(levelGroupType, levelNum, LevelGameParam.Open.ToString());

         if (levelData.Length != 1)
         {
            return false;
         }
         else
         {
            return levelData == "1";
         }
      }

      public static void SetLevelComplete(LevelGroupType levelGroupType, int levelNum, bool isOpen)
      {
         int param = isOpen ? 1 : 0;
         SetLevelData(levelGroupType, levelNum, LevelGameParam.Complete, param.ToString());
      }
      
      public static void SetLevelCompleteCurrent()
      {
         LevelManagerLevelParam levelParam = LevelManager.CurrentLevelParam;
         SetLevelData(levelParam.LevelGroupType, levelParam.LevelNum, LevelGameParam.Complete, 1.ToString());
      }
      
      public static bool GetLevelComplete(LevelGroupType levelGroupType, int levelNum)
      {
         if (!HasLevelData(levelGroupType, levelNum, LevelGameParam.Complete)) return false;
         
         string levelData = GetLevelData(levelGroupType, levelNum, LevelGameParam.Complete.ToString());
         
         if (levelData == null) return false;

         if (levelData.Length != 1)
         {
            return false;
         }
         else
         {
            return levelData == "1";
         }
      }
      
      public static int GetFirstLevelUncompleted(LevelGroupType levelGroupType)
      {
         LevelGroup levelGroup = LevelManager.GetLevelGroup(levelGroupType);
         
         for (int i = 1; i <= levelGroup.Levels.Count; i++)
         {
            bool completed = GetLevelComplete(levelGroupType, i);

            if (!completed) return i;
         }

         return 1;
      }
      
      public static void UnlockNextLevels(LevelGroupType levelGroupType, int levelNum)
      {
         if (levelGroupType == LevelGroupType.Classic)
         {
            SetLevelOpen(LevelGroupType.Classic, levelNum + 1, true);
            SetLevelOpen(LevelGroupType.Time, levelNum, true);
            
            SetLevelOpen(LevelGroupType.Thief, levelNum * 2, true);
            SetLevelOpen(LevelGroupType.Thief, (levelNum * 2) - 1, true);
         }
      }
      
      public static void UnlockNextLevels(LevelManagerLevelParam levelParam)
      {
         if(levelParam.UnlockNextLevelInGroup)
         {
            SetLevelOpen(levelParam.LevelGroupType, levelParam.LevelNum + 1, true);
         }

         for (int i = 0; i < levelParam.UnlockNextLevel.Length; i++)
         {
            SetLevelOpen(levelParam.UnlockNextLevel[i].LevelGroupType, levelParam.UnlockNextLevel[i].LevelNum, true);
            SetGroupData(levelParam.UnlockNextLevel[i].LevelGroupType,GroupGameParam.Warning.ToString(), "1");
         }
      }
   }
}