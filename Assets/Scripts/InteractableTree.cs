using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTree : MonoBehaviour
{
	public GameObject trunk;
	public List<GameObject> pieces;
	
	private Rigidbody rb;
	private int maxHealth = 100;
	private int health;

	private Coroutine shakeRoutine;
	
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeAll;
		health = maxHealth;

		foreach (var piece in pieces)
		{
			piece.SetActive(false);
		}
	}

	public void Cut(Vector3 hitForce)
	{
		health -= 30;
		if (health > 0)
		{
			if (shakeRoutine != null)
				StopCoroutine(shakeRoutine);
			shakeRoutine = StartCoroutine(ShakeRoutine());
			return;   
		}
		
		rb.constraints = RigidbodyConstraints.None;
		rb.AddForce(hitForce);

		LeanTween.delayedCall(3f, () =>
		{
			trunk.SetActive(false);
			LeanTween.delayedCall(0.1f, () =>
			{
				foreach (var piece in pieces)
				{
					piece.SetActive(true);
				}
			});
		});
	}

	private IEnumerator ShakeRoutine()
	{
		var dur = 0.5f;
		while (dur > 0f)
		{
			dur -= Time.deltaTime;
			var xRot = Random.Range(-0.5f, 0.5f);
			var zRot = Random.Range(-0.5f, 0.5f);
			transform.rotation = Quaternion.Euler(xRot, 0f, zRot);
			yield return null;
		}
		
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}
}
