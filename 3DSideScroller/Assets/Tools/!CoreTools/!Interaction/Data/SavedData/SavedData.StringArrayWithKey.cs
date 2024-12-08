using System.Collections.Generic;
using System.Linq;

namespace MgsTools.Data
{
    using UnityEngine;
    using System;
    
    public static partial class SavedData
    {
        [Serializable]
        public struct KeyValue
        {
            public string Key;
            public string Value;

            public KeyValue(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
        
        [Serializable]
        public struct KeyValuesArray
        {
            public KeyValue[] KeyValues;
        }
        
        public static class StringArrayWithKeyData
        {
            public static void SetValueByKey(string name, string key, string value)
            {
                string getSave = GetString(name);
                KeyValuesArray array = new KeyValuesArray();
                List<KeyValue> tempKeyValuesArray = new List<KeyValue>();

                if (getSave.Length != 0)
                {
                    array = (KeyValuesArray)OperationsParse.StringArrayWithType.GetValueFromArray(getSave, typeof(KeyValuesArray));

                    for (int i = 0; i < array.KeyValues.Length; i++)
                    {
                        if (array.KeyValues[i].Key == key)
                        {
                            array.KeyValues[i].Value = value;
                            SetString(name, OperationsParse.StringArrayWithType.SetValueToArray(array));
                            return;
                        }
                    }

                    tempKeyValuesArray = array.KeyValues.ToList();
                }

                tempKeyValuesArray.Add(new KeyValue(key, value));
                array.KeyValues = tempKeyValuesArray.ToArray();
                string s = OperationsParse.StringArrayWithType.SetValueToArray(array);
                SetString(name, s);
            }
            
            public static string GetValueByKey(string name, string key)
            {
                string getSave = GetString(name);

                if (getSave.Length != 0)
                {
                    KeyValuesArray array  = (KeyValuesArray)OperationsParse.StringArrayWithType.GetValueFromArray(getSave, typeof(KeyValuesArray));

                    for (int i = 0; i < array.KeyValues.Length; i++)
                    {
                        if (array.KeyValues[i].Key == key)
                        {
                            return array.KeyValues[i].Value.ToString();
                        }
                    }
                }

                return "";
            }
            
            public static void RemoveKeyValue(string name, string key)
            {
                string save = GetString(name);

                if (save.Length != 0)
                {
                    KeyValuesArray array = (KeyValuesArray)OperationsParse.StringArrayWithType.GetValueFromArray(save, typeof(KeyValuesArray));
                    List<KeyValue> tempKeyValuesArray = array.KeyValues.ToList();

                    for (int i = 0; i < tempKeyValuesArray.Count; i++)
                    {
                        if (tempKeyValuesArray[i].Key == key)
                        {
                            tempKeyValuesArray.Remove(tempKeyValuesArray[i]);
                            array.KeyValues = tempKeyValuesArray.ToArray();
                            string s = OperationsParse.StringArrayWithType.SetValueToArray(array);
                            SetString(name, s);
                            return;
                        }
                    }
                }
            }
            
            public static bool HasKey(string name, string key)
            {
                string save = GetString(name);

                if (save.Length != 0)
                {
                    KeyValuesArray array  = (KeyValuesArray)OperationsParse.StringArrayWithType.GetValueFromArray(save, typeof(KeyValuesArray));
                    for (int i = 0; i < array.KeyValues.Length; i++)
                    {
                        if (array.KeyValues[i].Key == key)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}