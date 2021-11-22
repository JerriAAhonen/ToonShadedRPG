using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
	public override string DisplayName { get; } = "Door";
	public override string InteractionText { get; } = "Open";
	public override bool CanInteract { get; } = true;

	public override void Interact()
	{
		base.Interact();
		transform.Rotate(0, 90, 0);
	}
}
