using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuStructure : MonoBehaviour
{
	public List<Transform> snapPoints;
	private Transform defaultSnapPoint;

	public Vector3 GetClosestSnapPoint(Vector3 buildPos)
	{
		var closestDist = Mathf.Infinity;
		var closestPoint = Vector3.zero;
		foreach (var snapPoint in snapPoints)
		{
			var dist = Vector3.Distance(buildPos, snapPoint.position);
			if (dist < closestDist)
			{
				closestDist = dist;
				closestPoint = snapPoint.position;
			}
		}

		return closestPoint;
	}

	public Vector3 GetDefaultSnapPoint()
	{
		if (defaultSnapPoint == null)
		{
			var defaultPoint = snapPoints[0];
			foreach (var snapPoint in snapPoints)
			{
				if (snapPoint.position.y < defaultPoint.position.y)
					defaultPoint = snapPoint;
			}

			defaultSnapPoint = defaultPoint;
		}
		

		return defaultSnapPoint.position;
	}
}
