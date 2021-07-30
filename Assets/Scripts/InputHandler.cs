using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class InputHandler : SingletonBehaviour<InputHandler>
{
	public bool IsAttacking { get; private set; }
	public bool IsAiming { get; private set; }

	public event Action Attack;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Attack?.Invoke();
			IsAttacking = true;
			LeanTween.cancel(gameObject, false);
			LeanTween.delayedCall(gameObject, PlayerAnimations.Instance.AttackAnimationLenght, () => IsAttacking = false);
		}

		IsAiming = Input.GetMouseButton(1);
	}
}
