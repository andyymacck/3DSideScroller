using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace MgsTools
{
	public static class OperationsParse
	{
		private const char c_splitChar = ';';
		private const char c_nullChar = '?';
		
		public static class IntArray
		{
			public static string IntArrayToString(int[] array, char splitChar = c_splitChar)
			{
				return string.Join(splitChar, array);
			}
			
			// can have some aloc issue
			public static int[] StringToIntArray(string intString, char splitChar = c_splitChar)
			{
				if (string.IsNullOrEmpty(intString)) return Array.Empty<int>();

				string[] stringArray = intString.Split(splitChar);
				int[] intArray = new int[stringArray.Length];
				for (int i = 0; i < stringArray.Length; i++)
				{
					if (stringArray[i] == c_nullChar.ToString())
					{
						intArray[i] = 0;
					}
					else if (int.TryParse(stringArray[i], out int value))
					{
						intArray[i] = value;
					}
					else
					{
						Debug.LogError("Can't convert: " + stringArray[i]);
						intArray[i] = 0;
					}
				}
				return intArray;
			}
			
			public static string ChangeValueInArrayString(string intString, int index, int value)
			{
				int[] array = StringToIntArray(intString);
				ChangeValueInArray(ref array, index, value);
				return IntArrayToString(array);
			}
			
			public static void ChangeValueInArray(ref int[] array, int index, int value)
			{
				if (array == null)
				{
					array = new int[index + 1];
					array[index] = value;
					return;
				}
				else if (index < 0)
				{
					Debug.LogError("Index is negative.");
					return; 
				}
				
				if (index >= array.Length)
				{
					int[] newArray = new int[index + 1];
					array.CopyTo(newArray, 0);
					newArray[index] = value;
					array = newArray;
					return;
				}
				
				array[index] = value;
			}
		}
		
		public static class IntNullableArray
		{
			public static string IntNullableArrayToString(int?[] array)
			{
				string[] stringArray = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					stringArray[i] = array[i].HasValue ? array[i].Value.ToString() : c_nullChar.ToString();
				}
				return string.Join(c_splitChar, stringArray);
			}
			
			public static int?[] StringToIntNullableArray(string arrayString)
			{
				if (string.IsNullOrEmpty(arrayString)) return Array.Empty<int?>();

				string[] stringArray = arrayString.Split(c_splitChar);
				int?[] intArray = new int?[stringArray.Length];
				for (int i = 0; i < stringArray.Length; i++)
				{
					if (stringArray[i].ToLower() == c_nullChar.ToString())
					{
						intArray[i] = null;
					}
					else if (int.TryParse(stringArray[i], out int value))
					{
						intArray[i] = value;
					}
					else
					{
						Debug.LogWarning("Can't convert: " + stringArray[i]);
						intArray[i] = null;
					}
				}
				
				return intArray;
			}
			
			public static string ChangeValueInArrayString(string intString, int index, int? value)
			{
				int?[] array = StringToIntNullableArray(intString);
				ChangeValueInArray(ref array, index, value);
				return IntNullableArrayToString(array);
			}

			public static void ChangeValueInArray(ref int?[] array, int index, int? newValue)
			{
				if (array == null)
				{
					array = new int?[index + 1];
					array[index] = newValue;
					return;
				}
				else if (index < 0)
				{
					Debug.LogError("Index is negative.");
					return;
				}
				
				if (index >= array.Length)
				{
					int?[] newArray = new int?[index + 1];
					array.CopyTo(newArray, 0);
					newArray[index] = newValue;
					array = newArray;
				}
				
				array[index] = newValue;
			}
		}
		
		public static class FloatArray
		{
			public static string FloatArrayToString(float[] array, char splitChar = c_splitChar)
			{
				string s = "";
				for (int i = 0; i < array.Length; i++)
				{
					s += array[i].ToString("f4");

					if (i != array.Length - 1)
					{
						s += splitChar.ToString();
					}
				}
				return s;
			}
			
			public static float[] StringToFloatArray(string intString, char splitChar = c_splitChar)
			{
				string[] s = intString.Split(splitChar);
				List<float> parsed = new List<float>();

				for (int i = 0; i < s.Length; i++)
				{
					if (!string.IsNullOrEmpty(s[i]))
					{
						try
						{
							float x = float.Parse(s[i]);
							parsed.Add(x);
						}
						catch (System.Exception)
						{
							throw;
						}
					}
				}

				return parsed.ToArray();
			}
		}
		
		public static class StringArray
		{
			public static string StringArrayToString(string[] array, char splitChar = c_splitChar)
			{
				string s = "";
				for (int i = 0; i < array.Length; i++)
				{
					s += array[i];

					if (i != array.Length - 1)
					{
						s += splitChar.ToString();
					}
				}
				return s;
			}

			public static string[] StringToStringArray(string strString, char splitChar = c_splitChar)
			{
				string[] s = strString.Split(splitChar);
				List<string> parsed = new List<string>();

				for (int i = 0; i < s.Length; i++)
				{
					parsed.Add(s[i]);
				}

				return parsed.ToArray();
			}

			public static bool IfExistInStringArray(string stringArrayAsString, string name)
			{
				string[] array = StringToStringArray(stringArrayAsString);
				return array.Contains(name);
			}
			
			public static string ChangeValueInArrayString(string intString, int index, string value)
			{
				string[] array = StringToStringArray(intString);
				ChangeValueInArray(ref array, index, value);
				return StringArrayToString(array);
			}

			public static void ChangeValueInArray(ref string[] array, int index, string newValue)
			{
				if (array == null)
				{
					array = new string[index + 1];
					array[index] = newValue;
					return;
				}
				else if (index < 0)
				{
					Debug.LogError("Index is negative.");
					return;
				}
				
				if (index >= array.Length)
				{
					string[] newArray = new string[index + 1];
					array.CopyTo(newArray, 0);
					newArray[index] = newValue;
					array = newArray;
				}
				
				array[index] = newValue;
			}
		} //End StringArray
		
		public static class StringArrayWithType
		{
			public static string SetValueToArray(object value)
			{
				return JsonUtility.ToJson(value);
			}
			
			public static object GetValueFromArray(string stringArray, Type type)
			{
				object value = JsonUtility.FromJson(stringArray, type);
				return value;
			}
			
		} //End StringArrayWithKey

		private static int[] SplitIntToParts(int value, int parts) //split price to parts to add as a coins / parts summ = value
		{
			int[] newParts = new int[parts];
			int prizePerPart = (int)((float)value / (float)parts);
			int total = 0;

			for (int i = 0; i < newParts.Length; i++)
			{
				int rest = Mathf.Clamp(value - total, 0, value);
				prizePerPart = Mathf.Clamp(prizePerPart, 0, rest);
				total += prizePerPart;

				if (i == newParts.Length - 1)
				{
					newParts[i] = rest;
				}
				else
				{
					newParts[i] = prizePerPart;
				}
			}

			return newParts;
		}
		
		/// <summary>
		/// 123 to 1/2/3 Array
		/// </summary>
		/// <param name="num"></param>
		/// <returns>Array[int]</returns>
		public static int[] SplitIntToNumbersArray(int num)
		{
			List<int> listOfInts = new List<int>();
			while(num > 0)
			{
				listOfInts.Add(num % 10);
				num = num / 10;
			}
			listOfInts.Reverse();
			return listOfInts.ToArray();
		}
	
		public static float FloatRound(float valueToRound, int numberAfterPoint)
		{
			double result  = Math.Round((double)(valueToRound), numberAfterPoint);
			return (float)result;
		}
	}
}