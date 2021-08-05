using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
	private bool tooltipActive;
	private float reachDistance = 10f;
	
	private void Update()
	{
		var ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out var hit, reachDistance))
		{
			var interactable = hit.collider.GetComponent<Interactable>();
			if (interactable != null)
			{
				CrosshairTooltips.Instance.ShowTargetDisplayName(interactable.DisplayName);
				tooltipActive = true;
			}
		}
		else if (tooltipActive)
		{ 
			CrosshairTooltips.Instance.Hide(); 
			tooltipActive = false;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position, transform.forward * reachDistance);
	}
}
