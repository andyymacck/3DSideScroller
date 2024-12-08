using System;
using System.Collections;
using UnityEngine;

namespace MgsTools
{
	public class OperationsRuntime : MonoBehaviour
	{
		//OperationsRuntime.RunWithDelay(() => { Method("1"); }, 1f);

		private static OperationsRuntime s_instance;
		public static OperationsRuntime Instance
		{
			get 
			{
				if (s_instance == null)
				{
					CreateInstance();
				}
				return s_instance;
			}
		}
		
		private static void CreateInstance()
		{
			GameObject go = new GameObject("OperationsRuntime");
			s_instance = go.AddComponent<OperationsRuntime>();
			DontDestroyOnLoad(go);
		}

		public static void RunWithDelay(Action action, float delay)
		{
			if (delay >= 0f && action != null)
			{
				Instance.StartCoroutine(RunWithDelayCoroutine(action, delay));
			}
		}

		public static void RunWhenActive(Action action, GameObject go, float maxWaitTime)
		{
			if (go != null && action != null)
			{
				Instance.StartCoroutine(RunWhenActiveCoroutine(action, go, maxWaitTime));
			}
		}

		private static IEnumerator RunWithDelayCoroutine(Action action, float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			action?.Invoke();
		}
		
		public static void RunWithDelayFrame(Action action, int framesAmount)
		{
			if (framesAmount >= 0 && action != null)
			{
				Instance.StartCoroutine(RunWithDelayFrameCoroutine(action, framesAmount));
			}
		}

		//To prevent issues on slow device
		private static IEnumerator RunWithDelayFrameCoroutine(Action action, int framesAmount)
		{
			int frames = 0;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			
			while (frames <= framesAmount)
			{
				frames++;
				yield return wait;
			}
			
			action?.Invoke();
		}

		private static IEnumerator RunWhenActiveCoroutine(Action action, GameObject go, float maxWaitTime)
		{
			float timer = 0f;
		
			while(go != null && timer < maxWaitTime && !go.activeInHierarchy)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			if(go != null && go.activeInHierarchy)
				action?.Invoke();
		}

		public static void EnableGameObjectWithDelay(GameObject go, bool enable, float delay)
		{
			if (go)
			{
				Instance.StartCoroutine(EnableGameObjectWithDelayCoroutine(go, enable, delay));
			}
		}

		public static IEnumerator EnableGameObjectWithDelayCoroutine(GameObject go, bool enable, float delay)
		{
			yield return new WaitForSeconds(delay);
			if(go) go.SetActive(enable);
		}
	}
}
