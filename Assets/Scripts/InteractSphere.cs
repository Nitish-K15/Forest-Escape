using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : Interactable
{
    [SerializeField] Transform Dest;
    private Rigidbody rb;
    private bool isHolding = false;
    public float throwForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact()
    {
        if (!isHolding)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            rb.useGravity = false;
            this.transform.position = Dest.position;
            this.transform.parent = GameObject.Find("Destination").transform;
            isHolding = true;
        }
        else
        {
            rb.freezeRotation = false;
            this.transform.parent = null;
            rb.useGravity = true;
            rb.AddForce(Dest.transform.forward * throwForce, ForceMode.Impulse);
            isHolding = false;
        }
    }
}
