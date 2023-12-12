using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : Block
{
    public float bounceHeight;

    private void Awake()
    {
        pRaycast = GameObject.FindGameObjectWithTag("RaycastStart").GetComponent<PlayerRaycast>();
        rotateOnSurface = false;
    }
    public override void Hover()
    {
        base.Hover();
        if (wallNormal.normalized != Vector3.up)
        {
            placeable = false;
            GetComponent<Renderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ThirdPersonController>() != null)
        {
            other.gameObject.GetComponent<ThirdPersonController>().ForceJump(bounceHeight);
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * bounceHeight * 20, ForceMode.Impulse);
        }
    }
}
