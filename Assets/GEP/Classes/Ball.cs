using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IObject
{
    public GameObject item;
    PlayerRaycast pRaycast;
    bool placeable = false;

    private void Awake()
    {
        pRaycast = GameObject.FindGameObjectWithTag("RaycastStart").GetComponent<PlayerRaycast>();
    }

    public void Hover()
    {
        transform.position = pRaycast.player.transform.position + pRaycast.GetDirection() * 2;
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2.001f, transform.rotation);
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

    public void Activate()
    {
        GetComponent<Collider>().enabled = false;
        Color m = GetComponent<Renderer>().material.color;
        m.a = 0.5f;
        GetComponent<Renderer>().material.color = m;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public bool Use()
    {
        if (placeable)
        {
            GetComponent<Collider>().enabled = true;
            Color m = GetComponent<Renderer>().material.color;
            m.a = 1;
            GetComponent<Renderer>().material.color = m;
            GetComponent<Rigidbody>().useGravity = true;

            GetComponent<Rigidbody>().AddForce(pRaycast.GetDirection() * 100, ForceMode.Impulse);
            return true;
        }

        return false;
    }
}
