using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class CameraRaycaster : SingletonBehaviour<CameraRaycaster>
{
	public LayerMask ignoreInBuildingMode;

	private bool tooltipActive;
	private float reachDistance = 10f;

	private Interactable targetedInteractable;
	
	public Vector3 BuildPos { get; private set; }

	private void Update()
	{
		var ray = new Ray(transform.position, transform.forward);
		
		//----------------------
		//-- BUILD MODE
		if (InputHandler.Instance.InBuildingMode)
		{
			if (Physics.Raycast(ray, out var hit, 30f, ~ignoreInBuildingMode))
			{
				BuildPos = hit.point;
			}
		}
		//----------------------
		//-- INTERACTION
		else
		{
			if (Physics.Raycast(ray, out var hit, reachDistance))
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
			}
		}
	}

	private void TryInteract()
	{
		if (targetedInteractable == null)
			return;
		
		targetedInteractable.Interact();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position, transform.forward * reachDistance);
	}
}
