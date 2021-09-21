using System.Collections.Generic;
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
		InputHandler.Instance.PreviewIndexChanged += OnChangePreview;
		active = InputHandler.Instance.InBuildingMode;
	}

	private void Update()
	{
		if (!active)
			return;

		SetPreview();
		MovePreview();
		HandleRotation();
	}

	private void SetPreview()
	{
		if (previewStructure == null)
			previewStructure = Instantiate(structures[structureIndex]);
	}

	private void OnChangePreview(int newIndex)
	{
		if (!active)
			return;

		structureIndex = Mathf.Clamp(newIndex, 0, 9);
		DeletePreview();
	}

	private void MovePreview()
	{
		if (previewStructure == null)
			return;
		
		var buildPos = CameraRaycaster.Instance.BuildPos;
		if (buildPos.structure == null)
		{
			drawDebugSphere = true;
			var sphereCastPos = buildPos.hit.point + previewStructure.DefaultSnapToOriginOffset;
			previewStructure.transform.position = GetClosestStructure(sphereCastPos, out var structure, out var hit) 
					? CalculateOppositeFacingSnappingPoint(structure, previewStructure, hit) 
					: CalculateDefaultSnapPointPlacement(previewStructure, buildPos.hit.point);
		}
		else
		{
			drawDebugSphere = false;
			previewStructure.transform.position = 
				CalculateOppositeFacingSnappingPoint(buildPos.structure, previewStructure, buildPos.hit);
		}
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

	private bool GetClosestStructure(Vector3 origin, out BuildMenuStructure closestStructure, out RaycastHit closestHit)
	{
		var radius = 1f;
		closestStructure = null;
		closestHit = default;

		var layerMask = 1 << LayerMask.NameToLayer("PlayerBuiltStructure");
		var cols = Physics.OverlapSphere(origin, radius, layerMask);

		debugSphereOrigin = origin;
		debugSphereRadius = radius;
		
		if (cols.Length > 0)
		{
			debugSphereColor = Color.green;
			var dist = Mathf.Infinity;
			var closestPointOnBounds = Vector3.zero;
			foreach (var col in cols)
			{
				var pointOnBounds = col.ClosestPointOnBounds(origin);
				var hitDistance = Vector3.Distance(origin, pointOnBounds);
				if (hitDistance < dist)
				{
					dist = hitDistance;
					closestStructure = col.GetComponent<BuildMenuStructure>();
					closestPointOnBounds = pointOnBounds;
				}
			}

			Physics.Raycast(origin, VectorUtils.GetDirVector(origin, closestPointOnBounds), out var hit, radius,
				layerMask);

			closestHit = hit;
			
			Debug.DrawRay(origin, VectorUtils.GetDirVector(origin, closestHit.point) * dist, Color.green);
			return true;
		}
		debugSphereColor = Color.gray;
		return false;
	}

	private bool drawDebugSphere;
	private Vector3 debugSphereOrigin;
	private float debugSphereRadius;
	private Color debugSphereColor;
	
	private void OnDrawGizmos()
	{
		if (!drawDebugSphere)
			return;

		Gizmos.color = debugSphereColor;
		Gizmos.DrawWireSphere(debugSphereOrigin, debugSphereRadius);
	}
}
