using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class BuildMenu : SingletonBehaviour<BuildMenu>
{
	public LayerMask playerBuiltStructures;
	public List<BuildMenuStructure> structures;
	private BuildMenuStructure activeStructure;
	private bool active;
	private RaycastHit[] hits;

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

		if (activeStructure == null)
			activeStructure = Instantiate(structures.First());

		var buildPos = CameraRaycaster.Instance.BuildPos;
		var hitsCount = Physics.SphereCastNonAlloc(buildPos, 1f, Vector3.up, hits, Mathf.Infinity, playerBuiltStructures);
		if (hitsCount > 0)
		{
			
		}
		else
		{
			var defaultStructurePos = activeStructure.GetDefaultSnapPoint();
			var originToSnapPointDelta = defaultStructurePos - activeStructure.transform.position;
			activeStructure.transform.position = CameraRaycaster.Instance.BuildPos - originToSnapPointDelta;
		}
	}

	private void TryPlaceStructure()
	{
		activeStructure.gameObject.layer = LayerMask.NameToLayer("PlayerBuiltStructure");
		activeStructure = null;
	}

	private void OnBuildModeToggle(bool activate)
	{
		active = activate;
		if (!activate && activeStructure != null)
		{
			Destroy(activeStructure.gameObject);
			activeStructure = null;
		}
	}
}
