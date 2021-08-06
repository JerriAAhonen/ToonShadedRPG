using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class InputHandler : SingletonBehaviour<InputHandler>
{
	public KeyCode InteractionKey { get; } = KeyCode.E;
	
	public bool IsAttacking { get; private set; }
	public bool IsAiming { get; private set; }
	public bool InBuildingMode { get; private set; }

	public event Action Attack;
	public event Action Interact;
	public event Action<bool> BuildModeToggle;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Attack?.Invoke();
			IsAttacking = true;
			LeanTween.cancel(gameObject, false);
			LeanTween.delayedCall(gameObject, PlayerAnimations.Instance.AttackAnimationLenght, () => IsAttacking = false);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("interact");
			Interact?.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			InBuildingMode = !InBuildingMode;
			BuildModeToggle?.Invoke(InBuildingMode);
		}

		IsAiming = Input.GetMouseButton(1);
	}
}
