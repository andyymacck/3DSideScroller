using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Operations
{
	private const char splitChar = '/';

	public static float RandomRangeDoubleSymbol(float min, float max) //return random float without zero point
	{
		float random = Random.Range(min, max);
		int symbol = Random.Range(0f, 1f) > 0.5f ? -1 : 1;
		return random * symbol;
	}

	public static bool ApproximatelyEqualToFloat(float value1, float value2, float maxDiff)
	{
		float diff = Mathf.Abs(value1 - value2);
		return diff < maxDiff;
	}

	public static T[] RamdomizeArray<T>(T[] array)
	{
		return RamdomizeList(array.ToList()).ToArray();
	}

	public static List<T> RamdomizeList<T>(List<T> list)
	{
		List<T> listTemp = new List<T>();
		listTemp.AddRange(list);
		List<T> listNew = new List<T>();

		while (listTemp.Count > 0)
		{
			T item = listTemp[Random.Range(0, listTemp.Count)];
			listNew.Add(item);
			listTemp.Remove(item);
		}

		return listNew;
	}
	
	public static int IntRandomRangeWithExceptions(int rangeMin, int rangeMax, int[] exclude)
	{
		List<int> range = new List<int>();

		int index = rangeMin;

		while (index <= rangeMax + 1)
		{
			range.Add(index);
			index++;
		}

		for (int i = 0; i < exclude.Length; i++)
		{
			range.Remove(exclude[i]);
		}

		return range.Count == 0 ? 0 : range[Random.Range(0, range.Count - 1)];
	}

	public static Vector3 TrajectoryPredictionAtTime(Vector3 start, Vector3 startVelocity, float time)
	{
		Vector3 pos = start + startVelocity * time + Physics.gravity * time * time * 0.5f;
		return pos;
	}

	public static Vector3 InputDirByCameraRotationY(Vector2 dir, Quaternion cameraRot)
	{
		Vector3 dir3 = new Vector3(dir.x, 0f, dir.y);
		float angle = cameraRot.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 dirCam = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)).normalized;
		Vector3 newDir = (dir3 * dirCam.z) + (new Vector3(dir3.z, 0f, -dir3.x) * dirCam.x);
		return newDir;
	}

	public static Vector3 V3InputDirByRotationY(Vector3 dir, Quaternion cameraRot) // dir.y not used
	{
		float angle = cameraRot.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 dirCam = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)).normalized;
		Vector3 newDir = (dir * dirCam.z) + (new Vector3(dir.z, 0f, -dir.x) * dirCam.x);
		return newDir;
	}

	public static Vector2 V2InputDirByRotationY(Vector2 dir, Quaternion cameraRot)
	{
		float angle = cameraRot.eulerAngles.y * Mathf.Deg2Rad;
		Vector2 dirCam = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
		Vector2 newDir = (dir * dirCam.y) + (new Vector2(dir.y, -dir.x) * dirCam.x);
		return newDir;
	}

	public static Vector2 V2MoveDirByObjRotationY(Vector3 dir, Quaternion ObjRot)
	{
		Vector3 cross = Vector3.Cross(dir, Vector3.up);
		float angle1 = Quaternion.Angle(ObjRot, Quaternion.LookRotation(dir));
		float angle2 = Quaternion.Angle(ObjRot, Quaternion.LookRotation(cross));
		float angle3 = angle2 > 90 && angle2 < 270 ? angle1 : -angle1;
		float angle = angle3 * Mathf.Deg2Rad;

		Vector2 forward = Vector2.up;
		Vector2 dirCam = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
		Vector2 newDir = (forward * dirCam.y) + (new Vector2(forward.y, -forward.x) * dirCam.x);
		newDir.x = -newDir.x;
		return newDir;
	}

	public static Quaternion DirRotation(Vector3 lookDir, Quaternion rot, Quaternion offset)
	{
		return Quaternion.LookRotation(lookDir) * Quaternion.Euler(rot.eulerAngles + offset.eulerAngles);
	}

	public static Vector3 RotationToDir(Quaternion rot)
    {
		return rot * Vector3.forward;
	}

	public static Color RandomColorByLerp( Color color1, Color color2 )
	{
		return Color.Lerp( color1, color2, UnityEngine.Random.Range(0f, 1f) );
	}

	public static Color RandomColorByRGBA( Color color1, Color color2 )
	{
		float r = UnityEngine.Random.Range( color1.r, color2.r );
		float g = UnityEngine.Random.Range( color1.g, color2.g );
		float b = UnityEngine.Random.Range( color1.b, color2.b );
		float a = UnityEngine.Random.Range( color1.a, color2.a );

		Color color = new Color( r, g, b, a );
		return color;
	}

	public static Vector3 Vector3LerpSeparate(Vector3 v1, Vector3 v2, float lx, float ly, float lz)
    {
		float x = Mathf.Lerp(v1.x, v2.x, lx);
		float y = Mathf.Lerp(v1.y, v2.y, ly);
		float z = Mathf.Lerp(v1.z, v2.z, lz);

		return new Vector3(x, y, z);
	}
	
	public static Vector3 RotationToDirection(Quaternion rotation)
	{
		return rotation * Vector3.forward;
	}
	
	public static Vector3 GetWorldPositionFromCam(Camera camera, Vector2 mousePos, float z)
	{
		Ray pos = camera.ScreenPointToRay(mousePos);
		Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
		ground.Raycast(pos, out float distance);
		return pos.GetPoint(distance);
	}
	
	public static float LerpToSin180Lerp(float lerp0To1)
	{
		return Mathf.Clamp01(Mathf.Sin((lerp0To1) * 180f * Mathf.Deg2Rad));
	}
	
	public static Vector3 CalcPositionByHypotenuse90(float targetHeight, float angle, Vector3 calcPoint)
	{
		float yDiff = calcPoint.y - targetHeight;
		float rad = (90f - angle) * Mathf.Deg2Rad;
		float cathetus = yDiff * Mathf.Tan(rad);
		return new Vector3(calcPoint.x, targetHeight, calcPoint.z - cathetus);
	}
	
	public static Vector2 ImageResize(Vector2 currentSize, Vector2 maxSize)
	{
		float imageAspect = currentSize.x / currentSize.y;
		float targetAspect = maxSize.x / maxSize.y;

		Vector2 size = new Vector2();

		if (imageAspect > targetAspect)
		{
			size.x = maxSize.x;
			size.y = maxSize.x / imageAspect;
		}
		else
		{
			size.y = maxSize.y;
			size.x = maxSize.y * imageAspect;
		}
		
		return size;
	}
}
