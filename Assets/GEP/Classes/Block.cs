using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour, IObject
{
    public GameObject item;
    PlayerRaycast pRaycast;

    private void Awake()
    {
        pRaycast = GameObject.FindGameObjectWithTag("RaycastStart").GetComponent<PlayerRaycast>();
    }

    public void Use()
    {
        GetComponent<Collider>().enabled = true;
    }

    public void Hover()
    {

        RaycastHit hitWall = pRaycast.GetCast();
        if (hitWall.collider != null)
        {
            GetComponent<Renderer>().enabled = true;
            Vector3 posWall = hitWall.point;
            transform.position = pRaycast.GetCast().point;
            transform.rotation = Quaternion.LookRotation(pRaycast.GetCast().normal);
            transform.position += pRaycast.GetCast().normal * (transform.localScale.x / 2);

            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2.1f, transform.rotation);
            int count = 0;
            while (hitColliders.Length > 0 && count < 3)
            {
                RaycastHit hit;
                GameObject colliderGameObject = hitColliders[0].gameObject;
                int colliderLayer = colliderGameObject.layer;
                colliderGameObject.layer = 10;
                bool boxCastHit = Physics.BoxCast(pRaycast.Pos(), transform.localScale / 2.1f, pRaycast.GetDirection(), out hit, transform.rotation, (transform.position - pRaycast.Pos()).magnitude, 10);
                if (boxCastHit)
                {
                    Vector3 normal = hit.normal;
                    if (Physics.Raycast(transform.position, -normal, out hit, transform.localScale.x / 2.1f))
                    {
                        float distance = (hit.point - transform.position).magnitude;
                        transform.position += normal * distance;
                    }
                }
                colliderGameObject.layer = colliderLayer;
                hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2.1f, transform.rotation);
                count++;
            }

            if (count > 3)
            {
                GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
