using System;
using UnityEngine;

namespace MgsTools.Data
{
	public static partial class SavedData
	{
#if ES3
		public static void RemoveSaveByKey(string nameToFind)
		{
			string[] keys = ES3.GetKeys("SaveFile.es3");
			int length = keys.Length;
			//Debug.Log(nameToFind);
			
			for (int i = 0; i < length; i++)
			{
				if (HasKeyText(nameToFind, keys[i]))
				{
					//Debug.Log(keys[i]);
					RemoveSave(keys[i]);
				}
			}
		}
#endif
		
		private static bool HasKeyText(string keyToFind, string savedKey, int indexSavedKey = 0, int indexKeyToFind = 0)
		{
			if (indexKeyToFind >= savedKey.Length) return false;
			if (indexSavedKey >= keyToFind.Length) return true;
			
			if (keyToFind[indexSavedKey] == savedKey[indexKeyToFind])
			{
				return HasKeyText(keyToFind, savedKey, indexSavedKey + 1, indexKeyToFind + 1);
			}
			else
			{
				return HasKeyText(keyToFind, savedKey, 0, indexKeyToFind + 1);
			}
		}
		
		private static void RemoveSave(string name)
		{
#if ES3
			ES3.DeleteKey(name);
#endif
			PlayerPrefs.DeleteKey(name);
		}
		
		public static bool IsExist(string name)
		{
			//return ES3.KeyExists(name);
			return PlayerPrefs.HasKey(name);
		}

		public static int GetInt(string name, int defaultValue = 0)
		{
			//return ES3.Load(name, defaultValue);
			return PlayerPrefs.GetInt(name, defaultValue);
		}

		public static void SetInt(string name, int value)
		{
#if ES3
			ES3.Save(name, value);
#endif
			PlayerPrefs.SetInt(name, value); PlayerPrefs.Save();
		}

		public static float GetFloat(string name, float defaultValue = 0f)
		{			
			//return ES3.Load(name, defaultValue);
			return PlayerPrefs.GetFloat(name, defaultValue);
		}

		public static void SetFloat(string name, float value)
		{
#if ES3
			ES3.Save(name, value);
#endif
			PlayerPrefs.SetFloat(name, value); PlayerPrefs.Save();
		}

		public static string GetString(string name, string defaultValue = "")
		{
			//return ES3.LoadString(name,defaultValue);
			return PlayerPrefs.GetString(name, defaultValue);
		}

		public static void SetString(string name, string value)
		{
#if ES3
			ES3.Save<string>(name, value);
#endif
			PlayerPrefs.SetString(name, value);
			PlayerPrefs.Save();
		}

		public static bool GetBool(string name, bool defaultValue = false)
		{
			if (IsExist(name)) return GetInt(name) == 1;
			else return defaultValue;
		}

		public static void SetBool(string name, bool value)
		{
#if ES3
			ES3.Save(name, value ? 1 : 0);
#endif
			PlayerPrefs.SetInt(name, value ? 1 : 0);
			PlayerPrefs.Save();
		}

		public static bool[] GetBoolArray(string name)
		{
			if (IsExist(name))
			{
				int[] array = IntArrayData.GetIntArray(name);
				bool[] boolArray = new bool[array.Length];
				for (int i = 0; i < boolArray.Length; i++)
				{
					boolArray[i] = array[i] == 1;
				}
				return boolArray;
			}
			else return new bool[0];
		}

		public static void SetBoolArray(string name, bool[] value)
		{
			int[] array = new int[value.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value[i] ? 1 : 0;
			}
			IntArrayData.SetIntArray(name, array);
		}
		
		public static void SetVector3(string name, Vector3 value)
		{
			float[] array = new float[3];
			array[0] = value.x; array[1] = value.y; array[2] = value.z;
			FloatArrayData.SetFloatArray(name, array);
		}

		public static Vector3 GetVector3(string name)
		{
			if (IsExist(name))
			{
				float[] array = FloatArrayData.GetFloatArray(name);

				if (array.Length == 3)
				{
					Vector3 vector3 = new Vector3(array[0], array[1], array[2]);
					return vector3;
				}
				else
					return Vector3.zero;
			}
			else
				return Vector3.zero;
		}

		public static void Delete(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}
	}
}
