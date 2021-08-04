using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTree : MonoBehaviour
{
	private Rigidbody rb;
	private int maxHealth = 100;
	private int health;
	
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeAll;
		health = maxHealth;
	}

	public void Cut(Vector3 hitForce)
	{
		health -= 30;
		if (health > 0)
		{
			StartCoroutine(ShakeRoutine());
			return;   
		}
		
		rb.constraints = RigidbodyConstraints.None;
		rb.AddForce(hitForce);
	}

	private IEnumerator ShakeRoutine()
	{
		
	}
}
