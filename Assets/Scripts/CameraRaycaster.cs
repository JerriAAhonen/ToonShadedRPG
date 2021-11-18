using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class CameraRaycaster : SingletonBehaviour<CameraRaycaster>
{
	[SerializeField] private LayerMask ignoreInBuildingMode;
	[SerializeField] private LayerMask ignoreInInteractionMode;
	[SerializeField] private float buildingModeReachDistance;
	[SerializeField] private float interactionModeReachDistance;

	private bool tooltipActive;

	private Interactable targetedInteractable;
	
	public RaycastHit? LookPos { get; private set; }

	private void Update()
	{
		var ray = new Ray(transform.position, transform.forward);
		
		//----------------------
		//-- BUILD MODE
		if (InputHandler.Instance.InBuildingMode)
		{
			if (Physics.Raycast(ray, out var hit, buildingModeReachDistance, ~ignoreInBuildingMode))
			{
				LookPos = hit;
			}
		}
		//----------------------
		//-- INTERACTION
		else
		{
			if (Physics.Raycast(ray, out var hit, interactionModeReachDistance, ~ignoreInInteractionMode))
			{
				LookPos = hit;
			}
			else
			{
				LookPos = null;
				CrosshairTooltips.Instance.Hide(); 
			}
		}
	}
	
	/*if (Physics.Raycast(ray, out var hit, reachDistance, ~ignoreInInteractionMode))
	{
		var interactable = hit.collider.GetComponent<Interactable>();
		if (interactable != null)
		{
			CrosshairTooltips.Instance.ShowTargetDisplayName(interactable.DisplayName, interactable.CanInteract, interactable.InteractionText);
			tooltipActive = true;

			targetedInteractable = interactable;
		}
		else 
			targetedInteractable = null;
	}
	else if (tooltipActive)
	{ 
		CrosshairTooltips.Instance.Hide(); 
		tooltipActive = false;
		targetedInteractable = null;
	}*/

	

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position, transform.forward * interactionModeReachDistance);
	}
}
