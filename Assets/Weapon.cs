using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float hitForce;

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;

        var forceDir = (other.transform.position - transform.position).normalized;
        enemy.Rigidbody.AddForce(forceDir * hitForce);
    }
}
