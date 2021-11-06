using System;
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
	public event Action<int> PreviewIndexChanged;
	public event Action PlaceBuilding;

	private void Update()
	{
		#region Mouse Input
		
		if (Input.GetMouseButtonDown(0))
		{
			if (InBuildingMode)
			{
				PlaceBuilding?.Invoke();
			}
			else
			{
				Attack?.Invoke();
				IsAttacking = true;
				LeanTween.cancel(gameObject, false);
				LeanTween.delayedCall(gameObject, PlayerAnimations.Instance.AttackAnimationLenght, () => IsAttacking = false);
			}
		}

		IsAiming = Input.GetMouseButton(1);
		
		#endregion
		
		#region Keyboard Alphabetic
		
		if (Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("interact");
			Interact?.Invoke();
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{
			InBuildingMode = !InBuildingMode;
			BuildModeToggle?.Invoke(InBuildingMode);
		}
		
		#endregion

		#region Keyboard Numeric

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PreviewIndexChanged?.Invoke(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PreviewIndexChanged?.Invoke(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			PreviewIndexChanged?.Invoke(2);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			PreviewIndexChanged?.Invoke(3);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			PreviewIndexChanged?.Invoke(4);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			PreviewIndexChanged?.Invoke(5);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			PreviewIndexChanged?.Invoke(6);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			PreviewIndexChanged?.Invoke(7);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			PreviewIndexChanged?.Invoke(8);
		}
		
		#endregion
	}
}
