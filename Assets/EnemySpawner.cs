using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;
	public Transform spawnPoint;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
	}
}
