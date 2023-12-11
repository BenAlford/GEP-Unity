using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject invSlotUIPrefab;
    Inventory inv;
    InventorySlotUI[] invSlots;

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        invSlots = new InventorySlotUI[inv.rows * inv.columns];
        for (int i = 0; i < invSlots.Length; i++)
        {
            GameObject newSlot = Instantiate(invSlotUIPrefab);
            newSlot.transform.SetParent(transform);
            newSlot.transform.localPosition = new Vector3(((i % inv.columns) * (size.x / inv.columns)) - (size.x / 2), ((i / inv.columns) * (size.y / inv.columns)) - (size.y / 2), newSlot.transform.localPosition.z);
            invSlots[i] = newSlot.GetComponent<InventorySlotUI>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
