using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class BuildMenu : SingletonBehaviour<BuildMenu>
{
	public List<BuildMenuStructure> structures;
	private BuildMenuStructure previewStructure;
	private bool active;
	private int structureIndex;

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

		SetPreview();
		ChangePreview();
		UpdatePreview();
		HandleRotation();
	}

	private void SetPreview()
	{
		if (previewStructure == null)
			previewStructure = Instantiate(structures[structureIndex]);
	}

	private void ChangePreview()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			structureIndex = 0;
			DeletePreview();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			structureIndex = 1;
			DeletePreview();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			structureIndex = 2;
			DeletePreview();
		}
	}

	private void UpdatePreview()
	{
		if (previewStructure == null)
			return;
		
		var buildPos = CameraRaycaster.Instance.BuildPos;
		previewStructure.transform.position = buildPos.structure != null 
			? CalculateOppositeFacingSnappingPoint(buildPos.structure, previewStructure, buildPos.hit) 
			: CalculateDefaultSnapPointPlacement(previewStructure, buildPos.hit.point);
	}

	private void HandleRotation()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			previewStructure.transform.Rotate(Vector3.up, 22.5f);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			previewStructure.transform.Rotate(Vector3.up, -22.5f);
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
			DeletePreview();
	}

	private void DeletePreview()
	{
		Destroy(previewStructure.gameObject);
		previewStructure = null;
	}
}
