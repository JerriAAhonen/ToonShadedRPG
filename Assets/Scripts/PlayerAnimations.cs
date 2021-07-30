﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerAnimations : SingletonBehaviour<PlayerAnimations>
{
	public AnimationClip attackAnimationClip;
	public Collider weaponCollider;
	private PlayerMovement pm;
	private Animator anim;
	
	private static readonly int Walking = Animator.StringToHash("Walking");
	private static readonly int Running = Animator.StringToHash("Running");

	public float AttackAnimationLenght => attackAnimationClip.length;

	private void Start()
	{
		pm = GetComponent<PlayerMovement>();
		anim = GetComponent<Animator>();

		pm.BecameIdle += OnBecameIdle;
		pm.StartedWalking += OnStartedWalking;
		pm.StartedRunning += OnStartedRunning;

		InputHandler.Instance.Attack += OnAttack;
	}

	private void OnBecameIdle()
	{
		anim.SetBool(Walking, false);
		anim.SetBool(Running, false);
	}

	private void OnStartedWalking()
	{
		anim.SetBool(Walking, true);
		anim.SetBool(Running, false);
		anim.Play("Walk");
	}

	private void OnStartedRunning()
	{
		anim.SetBool(Walking, false);
		anim.SetBool(Running, true);
		anim.Play("Run");
	}

	private Coroutine attackRoutine;
	
	private void OnAttack()
	{
		if (attackRoutine != null)
		{
			StopCoroutine(attackRoutine);
		}
		
		attackRoutine = StartCoroutine(AttackRoutine());
	}

	private IEnumerator AttackRoutine()
	{
		anim.Play("Attack");
		weaponCollider.enabled = true;

		var attackDuration = AttackAnimationLenght;
		while (attackDuration > 0)
		{
			attackDuration -= Time.deltaTime;
			yield return null;
		}

		weaponCollider.enabled = false;
		attackRoutine = null;
	}
}
