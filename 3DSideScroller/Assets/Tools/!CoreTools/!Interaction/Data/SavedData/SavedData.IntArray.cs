namespace MgsTools.Data
{
    using System;
    
    public static partial class SavedData
    {
        public static class IntArrayData
        {
            public static int[] GetIntArray(string name)
            {
                if (IsExist(name)) return OperationsParse.IntArray.StringToIntArray(GetString(name));
                return Array.Empty<int>();
            }

            public static void SetIntArray(string name, int[] value)
            {
                string s = OperationsParse.IntArray.IntArrayToString(value);
                SetString(name, s);
            }

            public static void SetIntValueInArray(string name, int index, int value)
            {
                SetString(name, OperationsParse.IntArray.ChangeValueInArrayString(GetString(name), index, value));
            }

            public static int GetIntArrayValue(string name, int index, int defaultValue = 0)
            {
                int[] array = GetIntArray(name);

                if (index >= array.Length) return defaultValue;

                return array[index];
            }
        }
    }
}
