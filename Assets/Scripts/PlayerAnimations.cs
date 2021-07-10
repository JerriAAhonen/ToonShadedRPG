using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	private PlayerMovement pm;
	private Animator anim;
	
	private static readonly int Walking = Animator.StringToHash("Walking");
	private static readonly int Running = Animator.StringToHash("Running");

	private void Start()
	{
		pm = GetComponent<PlayerMovement>();
		anim = GetComponent<Animator>();

		pm.BecameIdle += OnBecameIdle;
		pm.StartedWalking += OnStartedWalking;
		pm.StartedRunning += OnStartedRunning;
	}

	private void Update()
	{
		// Move this to the attack controller script
		if (Input.GetMouseButtonDown(0))
			OnAttack();
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

	private void OnAttack()
	{
		anim.Play("Attack");
	}
}
