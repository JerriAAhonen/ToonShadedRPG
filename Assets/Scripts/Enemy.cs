using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Rigidbody rb;
	private Animator anim;
	private int maxHealth = 100;
	private int health;
	private bool isAttacking;

	private static readonly int Idle = Animator.StringToHash("Idle");
	private static readonly int Walking = Animator.StringToHash("Walking");
	private static readonly int Attacking = Animator.StringToHash("Attacking");
	
	public bool Dead { get; set; }
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		Dead = false;
		health = maxHealth;
	}

	private void FixedUpdate()
	{
		var playerTransform = GetPlayer().transform;
		var dir = playerTransform.position - transform.position;
		var distance = dir.sqrMagnitude;

		if (distance < 2f)
		{
			anim.SetBool(Walking, false);
			anim.SetBool(Attacking, true);
			if (!isAttacking)
				StartCoroutine(AttackRoutine());
			return;
		}

		isAttacking = false;
		
		if (distance > 100f)
		{
			anim.SetBool(Walking, false);
			anim.SetBool(Attacking, false);
			anim.Play("Idle");
			return;
		}

		anim.SetBool(Walking, true);
		anim.SetBool(Attacking, false);
		
		transform.LookAt(playerTransform);
		
		dir.Normalize();
		rb.MovePosition(transform.position + dir * Time.deltaTime * 4f);
	}

	public void TakeDamage(int amount, Vector3 hitForce)
	{
		Debug.Log("Ouch!");
		health -= amount;
		if (health <= 0)
		{
			Dead = true;
			rb.AddForce(hitForce);
		}
	}

	private IEnumerator AttackRoutine()
	{
		isAttacking = true;
		while (isAttacking)
		{
			Attack();
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void Attack()
	{
		GetPlayer().TakeDamage();
	}
	
	private void FollowTargetWithRotation(Transform target, float distanceToStop, float speed)
	{
		if(Vector3.Distance(transform.position, target.position) > distanceToStop)
		{
			transform.LookAt(target);
			rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
		}
	}

	private PlayerInstance GetPlayer()
	{
		return PlayerInstance.Instance;
	}
}
