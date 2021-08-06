using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
	public LayerMask ignoreInBuildingMode;
	public GameObject testBuildingPrefab;
	private GameObject testBuilding;

	private bool tooltipActive;
	private float reachDistance = 10f;

	private Interactable targetedInteractable;

	private void Start()
	{
		InputHandler.Instance.Interact += TryInteract;
	}

	private void Update()
	{
		var ray = new Ray(transform.position, transform.forward);
		if (InputHandler.Instance.InBuildingMode)
		{
			if (Physics.Raycast(ray, out var hit, 30f, ~ignoreInBuildingMode))
			{
				if (testBuilding == null)
					testBuilding = Instantiate(testBuildingPrefab);

				testBuilding.transform.position = hit.point;
			}
		}
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
