using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	string DisplayName { get; }
	string InteractionText { get; }
	bool CanInteract { get; }

	void Interact();
}

public abstract class Interactable : MonoBehaviour, IInteractable
{
	public virtual string DisplayName { get; }
	public virtual string InteractionText { get; }
	public virtual bool CanInteract { get; } = true;

	public virtual void Interact() { }
}
