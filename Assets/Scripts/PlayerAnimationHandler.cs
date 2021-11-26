using System;
using System.Collections;
using UnityEngine;
using Util;

public class PlayerAnimationHandler : MonoBehaviour
{
	public static float AttackAnimationLength => 0.3f;
	
	
	[SerializeField] private Weapon weapon;
	[SerializeField] private Animator anim;
	
	private PlayerMovement pm;
	private Coroutine attackRoutine;

	private static readonly int vertical = Animator.StringToHash("Vertical");
	private static readonly int horizontal = Animator.StringToHash("Horizontal");


	private void Start()
	{
		pm = GetComponent<PlayerMovement>();

		InputHandler.Instance.Attack += OnAttack;

		weapon.ColliderEnabled = false;
	}

	public void UpdateAnimatorValues(float movementAmount)
	{
		anim.SetFloat(vertical, movementAmount, 0.1f, Time.deltaTime);
		anim.SetFloat(horizontal, movementAmount, 0.1f, Time.deltaTime);
	}

	private void OnAttack()
	{
		if (attackRoutine != null)
		{
			return;
		}
		
		attackRoutine = StartCoroutine(AttackRoutine());
	}

	private IEnumerator AttackRoutine()
	{
		anim.Play("Attack");
		weapon.ColliderEnabled = true;

		var attackDuration = AttackAnimationLength;
		while (attackDuration > 0)
		{
			attackDuration -= Time.deltaTime;
			yield return null;
		}

		weapon.ColliderEnabled = false;
		attackRoutine = null;
	}
}
