using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Rigidbody rb;
	private List<Vector3> hitsReceived = new List<Vector3>();

	public Rigidbody Rigidbody => rb;
	
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
}
