using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
	public static Vector3 GetDirVector(Vector3 from, Vector3 to)
	{
		return (to - from).normalized;
	}
}
