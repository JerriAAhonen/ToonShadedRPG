using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	string DisplayName { get; }
}

public abstract class Interactable : MonoBehaviour, IInteractable
{
	public virtual string DisplayName { get; }
}
