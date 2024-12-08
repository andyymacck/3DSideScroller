namespace MgsTools.Data
{
    using System;
    using UnityEngine;

    public static partial class SavedData
    {
        public static class StringArrayData
        {
            public static void SetArray(string name, string[] array)
            {
                string s = OperationsParse.StringArray.StringArrayToString(array);
                SetString(name, s);
            }

            public static string[] GetArray(string name)
            {
                string s = GetString(name);
                return OperationsParse.StringArray.StringToStringArray(s);
            }
            
            public static void SetValueInArray(string name, int index, string value)
            {
                SetString(name, OperationsParse.StringArray.ChangeValueInArrayString(GetString(name), index, value));
            }

            public static string GetArrayValue(string name, int index)
            {
                string[] array = GetArray(name);

                if (index >= array.Length || index < 0)
                {
                    Debug.Log("Index out of range");
                    return null;
                }

                return array[index];
            }
        }
    }
}