using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Rigidbody rb;
	private List<Vector3> hitsReceived = new List<Vector3>();
	
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
			return;
		
		var contactNormal = other.GetContact(0).normal;
		var flyDir = -contactNormal;
		rb.AddForce(flyDir * 50f, ForceMode.Impulse);
		
		hitsReceived.Add(flyDir);

		Debug.Log($"{other.transform.name}, dir: {flyDir}");
	}

	private void OnDrawGizmos()
	{
		foreach (var hit in hitsReceived)
		{
			Gizmos.DrawLine(transform.position, hit - transform.position);
		}
	}
}
