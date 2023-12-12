using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : ObjectBase
{
    PlayerRaycast pRaycast;

    private void Awake()
    {
        pRaycast = GameObject.FindGameObjectWithTag("RaycastStart").GetComponent<PlayerRaycast>();
    }

    public override void Hover()
    {
        transform.position = pRaycast.player.transform.position + pRaycast.GetDirection() * 2;
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, transform.localScale.x / 2.001f);
        if (hitColliders.Length > 0)
        {
            GetComponent<Renderer>().enabled = false;
            placeable = false;
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            placeable = true;
        }
    }

    public override void Activate()
    {
        GetComponent<Collider>().enabled = false;
        Color m = GetComponent<Renderer>().material.color;
        m.a = 0.5f;
        GetComponent<Renderer>().material.color = m;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Renderer>().enabled = false;
    }

    public override bool Use()
    {
        if (placeable)
        {
            GetComponent<Collider>().enabled = true;
            Color m = GetComponent<Renderer>().material.color;
            m.a = 1;
            GetComponent<Renderer>().material.color = m;
            GetComponent<Rigidbody>().useGravity = true;

            GetComponent<Rigidbody>().AddForce(pRaycast.GetDirection() * 150, ForceMode.Impulse);
            return true;
        }

        return false;
    }
}
