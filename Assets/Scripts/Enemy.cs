using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Rigidbody rb;
	private int maxHealth = 100;
	private int health;

	public bool Dead { get; set; }
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		Dead = false;
		health = maxHealth;
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
}
