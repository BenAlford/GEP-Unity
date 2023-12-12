using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour, IObject
{
    public GameObject item;
    protected bool placeable = false;

    virtual public bool Use()
    {
        return true;
    }

    virtual public void Hover()
    {

    }

    virtual public void Activate()
    {

    }

    virtual public void Break()
    {
        GameObject a = Instantiate(item);
        a.transform.position = transform.position;
        a.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        Destroy(gameObject);
    }
}
