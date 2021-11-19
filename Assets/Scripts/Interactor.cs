using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Util;

public class Interactor : MonoBehaviour
{
	[SerializeField] private TriggerCallbacks interactionCollider;

	private Interactable targetedInteractable;
	private HashSet<Interactable> interactablesInReach = new HashSet<Interactable>();

	private void Start()
	{
		InputHandler.Instance.Interact += TryInteract;
		interactionCollider.Enter += OnInteractionColliderEnter;
		interactionCollider.Exit += OnInteractionColliderExit;
	}

	private void Update()
	{
		var hit = CameraRaycaster.Instance.LookPos;
		if (!hit.HasValue)
			return;
		
		// Are we looking straight at an Interactable?
		var interactable = hit.Value.collider.GetComponent<Interactable>();
		if (interactable != null)
		{
			if (interactable != targetedInteractable)
				Hud.Instance.UnregisterInteractable(targetedInteractable);
			
			Hud.Instance.RegisterInteractable(interactable);
			targetedInteractable = interactable;
		}
		// Is there an Interactable within our interaction trigger?
		else if (interactablesInReach.Count > 0)
		{
			var closest = MiscUtil.GetClosestInteractable(hit.Value.point, interactablesInReach);
			if (closest != targetedInteractable)
				Hud.Instance.UnregisterInteractable(targetedInteractable);
			Hud.Instance.RegisterInteractable(closest);
			targetedInteractable = closest;
		}
		// Nothing to interact with
		else
		{
			Hud.Instance.UnregisterInteractable(targetedInteractable);
			targetedInteractable = null;
		}
	}

	private void OnInteractionColliderEnter(Collider other)
	{
		var interactable = other.GetComponent<Interactable>();
		if (interactable != null)
			interactablesInReach.Add(interactable);
	}

	private void OnInteractionColliderExit(Collider other)
	{
		var interactable = other.GetComponent<Interactable>();
		if (interactable == null)
			return;

		interactablesInReach.Remove(interactable);
		Hud.Instance.UnregisterInteractable(interactable);
	}
	
	private void TryInteract()
	{
		if (targetedInteractable == null)
			return;
		
		Hud.Instance.UnregisterInteractable(targetedInteractable);
		interactablesInReach.Remove(targetedInteractable);
		targetedInteractable.Interact();
	}
}
