using System.Collections.Generic;

namespace LevelManagerLoader
{
   using System;
   using MgsTools.Data;
   using MgsTools;
   using System.Linq;

   public class LevelManagerDataCore
   {
      protected const string c_id = "level_";
      protected const char c_splitChar = '/';

      public static bool HasValueByKey(LevelGroupType levelGroupType, int levelNum, string gameParam)
      {
         return SavedData.StringArrayWithKeyData.HasKey(GetLevelId(levelGroupType, levelNum), gameParam.ToString());
      }
      
      public static void SetLevelData(LevelGroupType levelGroupType, int levelNum, string gameParam, string[] param)
      {
         string s = "";

         for (int i = 0; i < param.Length; i++)
         {
            s += param[i];

            if (i != param.Length - 1)
            {
               s += c_splitChar.ToString();
            }
         }
         
         SavedData.StringArrayWithKeyData.SetValueByKey(GetLevelId(levelGroupType, levelNum), gameParam, s);
         //SetLevelData(levelGroupType, levelNum, gameParam, s);
      }

      public static void SetLevelData(LevelGroupType levelGroupType, int levelNum, LevelGameParam gameParam, string param)
      {
         SavedData.StringArrayWithKeyData.SetValueByKey(GetLevelId(levelGroupType, levelNum), gameParam.ToString(), param);
         //SetLevelData(levelGroupType, levelNum, gameParam.ToString(), param);
      }

      /*public static void SetLevelData(LevelGroupType levelGroupType, int levelNum, string gameParam, string param)
      {
         string[] rawData = ReadRawLevelParams(levelGroupType, levelNum);

         if (rawData == null || rawData.Length == 0) // if empty
         {
            string s = ConvertToLevelParam(gameParam, param);
            SaveRawLevelParams(levelGroupType, levelNum, new[] { s });
         }
         else
         {
            int indexToUpdate = -1;

            // Find the index of the data to update
            for (int i = 0; i < rawData.Length; i++)
            {
               string[] iData = ConvertFromLevelParam(rawData[i]);

               if (iData.Length == 0) continue;

               if (iData[0] == gameParam.ToString())
               {
                  indexToUpdate = i;
                  break;
               }
            }

            // Update the data
            if (indexToUpdate != -1)
            {
               rawData[indexToUpdate] = ConvertToLevelParam(gameParam, param);
            }
            else
            {
               // The data does not exist, add it
               string[] newRawData = new string[rawData.Length + 1];
               Array.Copy(rawData, newRawData, rawData.Length);
               newRawData[newRawData.Length - 1] = ConvertToLevelParam(gameParam, param);
               rawData = newRawData;
            }

            // Save the updated data
            SaveRawLevelParams(levelGroupType, levelNum, rawData);
         }
      }

      public static string[] GetLevelData(LevelGroupType levelGroupType, int levelNum, LevelGameParam gameParam)
      {
         return GetLevelData(levelGroupType, levelNum, gameParam.ToString());
      }
      
      public static string[] GetLevelData(LevelGroupType levelGroupType, int levelNum, string gameParam)
      {
         string[] rawData = ReadRawLevelParams(levelGroupType, levelNum);

         if (IsEmpty(rawData)) return null;

         for (int i = 0; i < rawData.Length; i++)
         {
            string[] levelData = ConvertFromLevelParam(rawData[i]);

            if (levelData.Length == 0) continue;

            if (levelData[0] == gameParam)
            {
               // Return array without the first element
               return levelData.Skip(1).ToArray();
            }
         }

         return null;
      }*/

      public static string GetLevelData(LevelGroupType levelGroupType, int levelNum, string gameParam)
      {
         return SavedData.StringArrayWithKeyData.GetValueByKey(GetLevelId(levelGroupType, levelNum), gameParam);
      }
      
      public static string[] GetLevelData(LevelGroupType levelGroupType, int levelNum, LevelGameParam gameParam)
      {
         if (!HasValueByKey(levelGroupType, levelNum, gameParam.ToString())) return null;
         
         string save = GetLevelData(levelGroupType, levelNum, gameParam.ToString());
         string[] levelData = ConvertFromLevelParam(save);
         
         return levelData;
      }

      ///
      private static string ConvertToLevelParam(string gameParam, string param)
      {
         return OperationsParse.StringArray.StringArrayToString(new[] { gameParam, param }, c_splitChar);
      }

      private static string[] ConvertFromLevelParam(string param)
      {
         return OperationsParse.StringArray.StringToStringArray(param, c_splitChar);
      }

      protected static string[] ReadRawLevelParams(LevelGroupType levelGroupType, int levelNum)
      {
         string[] rawData = SavedData.StringArrayData.GetArray(GetLevelId(levelGroupType, levelNum));

         if (rawData.Length == 0) return null;

         return rawData;
      }

      /*private static void SaveRawLevelParams(LevelGroupType levelGroupType, int levelNum, string[] levelParams)
      {
         SavedData.StringArrayData.SetArray(GetLevelId(levelGroupType, levelNum), levelParams);
      }*/

      private static string GetLevelId(LevelGroupType levelGroupType, int levelNum)
      {
         return c_id + levelGroupType + "_" + levelNum;
      }

      protected static bool IsEmpty(string[] param)
      {
         return param == null || param.Length == 0;
      }

      public static void ConvertOldSave()
      {
         int levelNumMax = 25;
         string saveName;

         foreach (LevelGroupType levelGroupType in (LevelGroupType[])Enum.GetValues(typeof(LevelGroupType)))
         {
            for (int i = 0; i < levelNumMax; i++)
            {
               saveName = GetLevelId(levelGroupType, i);
               if (SavedData.IsExist(saveName))
               {
                  string value = SavedData.GetString(saveName);
                  
                  if (value[0].ToString() == "{") return;
                  
                  string[] rawData = ReadRawLevelParams(levelGroupType, i);
                  
                  SavedData.KeyValuesArray array = new SavedData.KeyValuesArray();
                  List<SavedData.KeyValue> tempKeyValuesArray = new List<SavedData.KeyValue>();
                  
                  for (int raw = 0; raw < rawData.Length; raw++)
                  {
                     string[] iData = ConvertFromLevelParam(rawData[raw]);
                     
                     if (iData.Length == 0) continue;

                     if (iData[0] == LevelGameParam.Open.ToString())
                     {
                        tempKeyValuesArray.Add(new SavedData.KeyValue(LevelGameParam.Open.ToString(),iData[1]));
                     }
                     if (iData[0] == LevelGameParam.Main.ToString())
                     {
                        string valueMain = iData[1] + c_splitChar + iData[2];
                        tempKeyValuesArray.Add(new SavedData.KeyValue(LevelGameParam.Main.ToString(),valueMain));
                     }                     
                     if (iData[0] == LevelGameParam.Complete.ToString())
                     { 
                        tempKeyValuesArray.Add(new SavedData.KeyValue(LevelGameParam.Complete.ToString(),iData[1]));
                     }
                     if (iData[0] == LevelGameParam.Timer.ToString())
                     {
                        tempKeyValuesArray.Add(new SavedData.KeyValue(LevelGameParam.Timer.ToString(),iData[1]));
                     }
                     if (iData[0] == LevelGameParam.Coin.ToString())
                     {
                        string valueMain = iData[1] + c_splitChar + iData[2];
                        tempKeyValuesArray.Add(new SavedData.KeyValue(LevelGameParam.Coin.ToString(),valueMain));
                     }                     
                     if (iData[0] == LevelGameParam.Gold.ToString())
                     {
                        string valueMain = iData[1] + c_splitChar + iData[2];
                        tempKeyValuesArray.Add(new SavedData.KeyValue(LevelGameParam.Gold.ToString(),valueMain));
                     }
                     
                     array.KeyValues = tempKeyValuesArray.ToArray();
                     string s = OperationsParse.StringArrayWithType.SetValueToArray(array);
                     UnityEngine.Debug.Log($"saveName {saveName}  KeyValue {s}");
                     SavedData.SetString(saveName, s);
                  }
               }
            }
         }
      }
   }
}
