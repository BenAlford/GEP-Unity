using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject invSlotUIPrefab;
    InventorySlotUI[] invSlots;

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        Inventory inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        invSlots = new InventorySlotUI[inv.rows * inv.columns];
        for (int i = 0; i < invSlots.Length; i++)
        {
            GameObject newSlot = Instantiate(invSlotUIPrefab);
            newSlot.transform.SetParent(transform);
            newSlot.transform.localPosition = new Vector3(((i % inv.columns) * (size.x / inv.columns)) - (size.x / 2) + 100, ((i / inv.columns) * ((size.y + 200) / inv.columns)) - (size.y / 2) + 100, newSlot.transform.localPosition.z);
            invSlots[i] = newSlot.GetComponent<InventorySlotUI>();
        }
    }

    public void SetSlotItem(int slot, ItemDef item, int size)
    {
        Debug.Log("f");
        invSlots[slot].SetItem(item, size);
    }

    public void SetSlotItemSize(int slot, int size)
    {
        invSlots[slot].SetSize(size);
    }

    public void ClearSlot(int slot)
    {
        invSlots[slot].Hide();
    }    
}
