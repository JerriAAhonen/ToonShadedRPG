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
		
		var (raycastHit, _) = CameraRaycaster.Instance.BuildPos;
		previewStructure.transform.position = CalculateDefaultSnapPointPlacement(previewStructure, raycastHit.point);
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
