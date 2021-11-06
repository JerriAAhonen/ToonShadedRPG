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
	private float movementSpeed = 2.5f;
	private bool isAttacking;

	private static readonly int Walking = Animator.StringToHash("Walking");
	
	public bool Dead { get; set; }
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		Dead = false;
		health = maxHealth;
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

	private void FixedUpdate()
	{
		if (Dead)
			return;
		
		var playerTransform = GetPlayer().transform;
		var dir = playerTransform.position - transform.position;
		var distance = Vector3.Distance(playerTransform.position, transform.position);

		if (distance < 5f)
		{
			anim.SetBool(Walking, false);
			return;
			anim.SetBool(Walking, false);
			if (!isAttacking)
				StartCoroutine(AttackRoutine());
			return;
		}

		isAttacking = false;
		
		if (distance > 100f)
		{
			anim.SetBool(Walking, false);
			return;
		}

		anim.SetBool(Walking, true);
		
		transform.LookAt(playerTransform);
		
		dir.Normalize();
		rb.MovePosition(transform.position + dir * (Time.deltaTime * movementSpeed));
	}

	public void TakeDamage(int amount, Vector3 hitForce)
	{
		Debug.Log($"{name} took {amount} damage");
		anim.Play("Attacking");
		health -= amount;
		if (health <= 0)
		{
			Debug.Log($"{name} died");
			Dead = true;
			rb.constraints = RigidbodyConstraints.None;
			rb.AddForce(hitForce);
		}
	}

	private IEnumerator AttackRoutine()
	{
		isAttacking = true;
		while (isAttacking)
		{
			anim.Play("Attack");
			Attack();
			yield return new WaitForSeconds(1f);
		}
	}

	private void Attack()
	{
		GetPlayer().TakeDamage();
	}

	private PlayerInstance GetPlayer()
	{
		return PlayerInstance.Instance;
	}
}
