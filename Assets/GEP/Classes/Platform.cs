using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Platform : ObjectBase
{
    GameObject player;
    bool active = false;
    public float time_to_fade;
    float timer;
    Renderer render;
    Color alphaColour;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        render = GetComponent<Renderer>();
        alphaColour = render.material.color;
    }

    public override void Hover()
    {
        
    }

    public override void Activate()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    public override bool Use()
    {
        active = true;
        transform.position = player.transform.position;
        transform.position -= new Vector3 (0,transform.localScale.y / 2,0);
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;

        return true;
    }

    private void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;
            alphaColour.a = 1 - (timer / time_to_fade);
            if (alphaColour.a < 0)
            {
                Break();
                return;
            }
            render.material.color = alphaColour;
        }
    }
}
