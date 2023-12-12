using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour, IObject
{
    public GameObject item;
    PlayerRaycast pRaycast;

    bool placeable = false;

    private void Awake()
    {
        pRaycast = GameObject.FindGameObjectWithTag("RaycastStart").GetComponent<PlayerRaycast>();
    }

    public bool Use()
    {
        if (placeable)
        {
            GetComponent<Collider>().enabled = true;
            Color m = GetComponent<Renderer>().material.color;
            m.a = 1;
            GetComponent<Renderer>().material.color = m;
            return true;
        }
        return false;
    }

    public void Hover()
    {

        RaycastHit hitWall = pRaycast.GetCast();
        if (hitWall.collider != null)
        {
            GetComponent<Renderer>().enabled = true;
            Vector3 posWall = hitWall.point;
            Vector3 wallNormal = hitWall.normal;
            transform.position = pRaycast.GetCast().point;
            transform.rotation = Quaternion.LookRotation(pRaycast.GetCast().normal);
            transform.position += pRaycast.GetCast().normal * (transform.localScale.x / 2);

            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2.001f, transform.rotation);
            //int count = 0;
            //while (hitColliders.Length > 0 && count < 3)
            //{

            //    Vector3 closestPoint = hitColliders[0].ClosestPoint(transform.position);
            //    Vector3 positionDifference = (closestPoint - transform.position);
            //    Vector3 overlapDirection = positionDifference.normalized;

            //    RaycastHit hit;
            //    int layer = gameObject.layer;

            //    gameObject.layer = 10;
            //    float raycastDistance = 10;
            //    Vector3 rayStart = transform.position + overlapDirection * raycastDistance;
            //    Vector3 rayDirection = -overlapDirection;

            //    if (Physics.Raycast(rayStart, rayDirection, out hit, Mathf.Infinity, 1 << 10))
            //    {
            //        //https://math.stackexchange.com/questions/1521128/given-a-line-and-a-point-in-3d-how-to-find-the-closest-point-on-the-line
            //        float t = (((transform.position - posWall).magnitude * (posWall - closestPoint).magnitude) / ((transform.position - posWall).magnitude * (transform.position - posWall).magnitude));
            //        Vector3 third = posWall - t * (transform.position - posWall);

            //        float distance = (third - closestPoint).magnitude;
            //        Debug.Log(hit.normal);
            //        transform.position += -hit.normal * distance;
            //    }

            //    count++;
            //    hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2.001f, transform.rotation);
                //Vector3 normal = hitColliders[0].ClosestPoint(transform.position);
                //if (Physics.Raycast(transform.position, -normal, out hit, transform.localScale.x / 1.999f))
                //{
                //    float distance = (transform.localScale.x / 2) - (hit.point - transform.position).magnitude;
                //    transform.position += normal * distance;
                //}
                //hitColliders[0]
                //Debug.Log("2");
                //RaycastHit hit;
                //GameObject colliderGameObject = hitColliders[0].gameObject;
                //int colliderLayer = colliderGameObject.layer;
                //colliderGameObject.layer = 10;
                //bool boxCastHit = Physics.BoxCast(pRaycast.Pos(), transform.localScale, pRaycast.GetDirection(), out hit, transform.rotation, (transform.position - pRaycast.Pos()).magnitude, 1 << 10);
                //if (boxCastHit)
                //{
                //    Vector3 normal = hit.normal;
                //if (Physics.Raycast(transform.position, -normal, out hit, transform.localScale.x / 1.999f))
                //{
                //    float distance = (transform.localScale.x / 2) - (hit.point - transform.position).magnitude;
                //    transform.position += normal * distance;
                //}
                //}
                //colliderGameObject.layer = colliderLayer;
                //hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2.001f, transform.rotation);
                //count++;
            //}

            //if (count > 3)
            //{
            //    GetComponent<Renderer>().enabled = false;
            //}

            if (hitColliders.Length > 0)
            {
                placeable = false;
                GetComponent<Renderer>().enabled = false;
            }
            else
            {
                placeable = true;
                GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            placeable = false;
            GetComponent<Renderer>().enabled = false;
        }
    }

    public void Activate()
    {
        GetComponent<Collider>().enabled = false;
        Color m = GetComponent<Renderer>().material.color;
        m.a = 0.5f;
        GetComponent<Renderer>().material.color = m;
    }
}
