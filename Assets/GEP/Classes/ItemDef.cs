using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemDef", order = 1)]
public class ItemDef : ScriptableObject
{
    public int max_stack_size;
    public string item_name;
    public string description;
    public Sprite sprite;
    public GameObject prefab;
}
