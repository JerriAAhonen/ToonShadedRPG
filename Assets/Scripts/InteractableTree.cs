using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTree : MonoBehaviour
{
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Cut(Vector3 hitForce)
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(hitForce);
    }
}
