﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float hitForce;
    public ParticleSystem hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (hitEffect != null)
            {
                var effect = Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
                effect.Play(true);
            }
            HitEnemy(enemy);
        }

        var tree = other.GetComponent<InteractableTree>();
        if (tree != null)
            HitTree(tree);
    }

    private void HitEnemy(Enemy enemy)
    {
        var forceDir = (enemy.transform.position - transform.position).normalized;
        forceDir += Vector3.up * 1f;
        enemy.TakeDamage(50, forceDir * hitForce);
    }

    private void HitTree(InteractableTree tree)
    {
        var forceDir = (tree.transform.position - transform.position).normalized;
        tree.Cut(forceDir * hitForce);
    }
}
