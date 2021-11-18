using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
	[SerializeField] private Collider interactionCollider;

	private Interactable targetedInteractable;
	private bool tooltipActive;

	private void Start()
	{
		InputHandler.Instance.Interact += TryInteract;
	}

	private void Update()
	{
		var hit = CameraRaycaster.Instance.LookPos;
		if (!hit.HasValue)
			return;
			
		var interactable = hit.Value.collider.GetComponent<Interactable>();
		if (interactable != null && targetedInteractable != interactable)
			OnNewInteractable(interactable);
		else if (interactable == null)
			OnNewInteractable(null);
		
		// TODO: Get collider triggers from gatherers
	}
	
	private void TryInteract()
	{
		if (targetedInteractable == null)
			return;
		
		targetedInteractable.Interact();
	}
	

	private void OnNewInteractable(Interactable interactable)
	{
		if (interactable == null)
		{
			CrosshairTooltips.Instance.Hide(); 
			tooltipActive = false;
			targetedInteractable = null;
			return;
		}
		
		targetedInteractable = interactable;
		CrosshairTooltips.Instance.ShowTargetDisplayName(interactable.DisplayName, interactable.CanInteract, interactable.InteractionText);
		tooltipActive = true;
	}
}
