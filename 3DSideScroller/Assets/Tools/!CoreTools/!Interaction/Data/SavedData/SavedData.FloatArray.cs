namespace MgsTools.Data
{
    using System;

    public static partial class SavedData
    {
        public static class FloatArrayData
        {
            // float array
            public static float[] GetFloatArray(string name)
            {
                if (IsExist(name)) return OperationsParse.FloatArray.StringToFloatArray(GetString(name));
                return Array.Empty<float>();
            }

            public static void SetFloatArray(string name, float[] value)
            {
                string s = OperationsParse.FloatArray.FloatArrayToString(value);
                SetString(name, s);
            }
        }
    }
}