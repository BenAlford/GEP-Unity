using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public GameObject player;
    public RaycastHit GetCast()
    {
        transform.LookAt(player.transform.position);
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 4, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        return hit;
    }

    public Vector3 GetDirection()
    {
        return transform.forward;
    }

    public Vector3 Pos()
    {
        transform.LookAt(player.transform.position);
        return transform.position;
    }
}
