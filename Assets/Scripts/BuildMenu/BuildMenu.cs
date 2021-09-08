using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class BuildMenu : SingletonBehaviour<BuildMenu>
{
	public LayerMask playerBuiltStructures;
	public List<BuildMenuStructure> structures;
	private BuildMenuStructure previewStructure;
	private bool active;
	private RaycastHit[] hits = new RaycastHit[100];

	private void Start()
	{
		InputHandler.Instance.BuildModeToggle += OnBuildModeToggle;
		InputHandler.Instance.PlaceBuilding += TryPlaceStructure;
		active = InputHandler.Instance.InBuildingMode;
	}

	private void Update()
	{
		if (!active)
			return;

		if (previewStructure == null)
			previewStructure = Instantiate(structures.First());

		var buildPos = CameraRaycaster.Instance.BuildPos;
		if (buildPos.structure != null)
		{
			previewStructure.transform.position =
				CalculateOppositeFacingSnappingPoint(buildPos.structure, previewStructure, buildPos.hit);
		}
		else
		{
			previewStructure.transform.position = CalculateDefaultSnapPointPlacement(previewStructure, buildPos.hit.point);
		}
	}

	private Vector3 CalculateDefaultSnapPointPlacement(BuildMenuStructure structure, Vector3 buildPos)
	{
		var activeStructurePos = structure.transform.position;
		var defaultStructurePos = structure.GetDefaultSnapPoint();
		var originToSnapPointDelta = defaultStructurePos - activeStructurePos;
		return buildPos - originToSnapPointDelta;
	}

	private Vector3 CalculateOppositeFacingSnappingPoint(
		BuildMenuStructure targetedStructure, 
		BuildMenuStructure newStructure, 
		RaycastHit hit)
	{
		var biggestAngle = 0f;
		Transform result = null;
		foreach (var snapPoint in newStructure.snapPoints)
		{
			var angle = Vector3.Angle(hit.normal, snapPoint.forward);
			if (angle > biggestAngle)
			{
				biggestAngle = angle;
				result = snapPoint;
			}
		}

		if (result == null)
			result = newStructure.snapPoints[0];

		var closestSnappingPointOnTargetedStructure = targetedStructure.GetClosestSnapPoint(hit.point);
		var buildPosToNewStructureOrigin = newStructure.transform.position - hit.point;
		var buildPosToNewStructureSnap = result.position - buildPosToNewStructureOrigin;
		var buildPosToTargetStructureSnap = closestSnappingPointOnTargetedStructure - buildPosToNewStructureSnap;
		
		return hit.point + buildPosToTargetStructureSnap;
	}

	private void TryPlaceStructure()
	{
		previewStructure.gameObject.layer = LayerMask.NameToLayer("PlayerBuiltStructure");
		previewStructure = null;
	}

	private void OnBuildModeToggle(bool activate)
	{
		active = activate;
		if (!activate && previewStructure != null)
		{
			Destroy(previewStructure.gameObject);
			previewStructure = null;
		}
	}
}
