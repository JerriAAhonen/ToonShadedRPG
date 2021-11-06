using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<Collider> hitColliders = new List<Collider>();
    
    public float hitForce;
    public Collider col;
    public ParticleSystem hitEffect;

    public bool ColliderEnabled
    {
        get => col.enabled;
        set
        {
            col.enabled = value;
            if (!value)
                hitColliders.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (hitColliders.Contains(other))
                return;
            hitColliders.Add(other);
            
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
