namespace MgsTools.Data
{
    using System;
    using UnityEngine;

    public static partial class SavedData
    {
        public static class IntNullableArrayData
        {
            public static int?[] GetArray(string name)
            {
                if (IsExist(name)) return OperationsParse.IntNullableArray.StringToIntNullableArray(GetString(name));
                return Array.Empty<int?>();
            }

            public static void SetArray(string name, int?[] value)
            {
                string s = OperationsParse.IntNullableArray.IntNullableArrayToString(value);
                SetString(name, s);
            }

            public static void SetValueInArray(string name, int index, int? value)
            {
                SetString(name, OperationsParse.IntNullableArray.ChangeValueInArrayString(GetString(name), index, value));
            }

            public static int? GetArrayValue(string name, int index)
            {
                int?[] array = GetArray(name);

                if (index >= array.Length || index < 0)
                {
                    Debug.Log($"Index out of range:{name}:{index}");
                    return null;
                }

                return array[index];
            }
        }
    }
}