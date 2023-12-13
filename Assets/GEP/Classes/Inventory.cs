using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;

//struct InventorySlot
//{
//    public ItemDef itemDef;
//    public int number;
//}

public class InventorySlot
{
    private bool empty = true;
    private ItemDef inventoryItem;
    private int size = 1;

    public void SetItem(ItemDef newItem, int newSize)
    {
        inventoryItem = newItem;
        size = newSize;
        empty = false;
    }

    public ItemDef GetItem()
    {
        return inventoryItem;
    }

    public void Clear()
    {
        empty = true;
    }

    public bool TryAdd()
    {
        if (size < inventoryItem.max_stack_size)
        {
            size++;
            return true;
        }
        return false;
    }

    public bool TryRemove()
    {
        if (size > 1)
        {
            size--;
            return true;
        }
        return false;
    }

    public bool IsEmpty()
    {
        return empty;
    }

    public int GetSize()
    {
        return size;
    }
}

public class Inventory : MonoBehaviour
{
    public int rows;
    public int columns;
    public int selected_slot = 0;
    int next_free = 0;
    public ItemDef start_item;
    InventorySlot[] inventory;
    Dictionary<ItemDef, List<int>> inventoryDictionary = new Dictionary<ItemDef, List<int>>();
    InventoryUIManager invUIManager;

    GameObject currentObject = null;
    IObject currentObjectInterface = null;
    bool currentObjectHoverable;

    InventorySlotUI ui;

    private void Awake()
    {
        invUIManager = GameObject.FindGameObjectWithTag("InventoryUIManager").GetComponent<InventoryUIManager>();

        // initialise the array
        inventory = new InventorySlot[rows * columns];
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventorySlot();
        }
    }

    private void Start()
    {
        //inventory[0].SetItem(start_item, 5);
        //Stack<int> a = new Stack<int>();
        //a.Push(0);
        //inventoryDictionary.Add(start_item, a);

    }

    public void AddItem(ItemDef item)
    {
        List<int> slots;

        // checks if the item is already in inventry, gets the slots of it if it is
        if (inventoryDictionary.TryGetValue(item, out slots))
        {
            // tries to add the item to the slot
            bool added_item = inventory[slots.Last()].TryAdd();
            if (added_item)
            {
                invUIManager.SetSlotItemSize(slots.Last(), inventory[slots.Last()].GetSize());
            }

            // if it failed try to add it to a previous slot
            if (!added_item && slots.Count > 1)
            {
                for (int i = 0; i < slots.Count - 1; i++)
                {
                    if (inventory[slots[i]].TryAdd())
                    {
                        // resorts the list to put this slot at the end 
                        int pos = slots[i];
                        slots.RemoveAt(i);
                        slots.Add(pos);
                        added_item = true;
                        invUIManager.SetSlotItemSize(pos, inventory[pos].GetSize());
                        break;
                    }
                }
            }

            // if it failed and the inventry isn't full
            if (!added_item && next_free != -1)
            {

                // add it to the next available slot
                if (!added_item)
                {
                    slots.Add(next_free);
                    inventory[next_free].SetItem(item, 1);


                    invUIManager.SetSlotItem(next_free, item, 1);

                    if (next_free == selected_slot)
                    {
                        SetupCurrentObject();
                    }

                    // find next available slot for next time
                    FindNextFree();
                }
            }
        }
        // if it does not exist yet in the inventory and it is not full
        else if (next_free != -1)
        {
            // add it to the dictionary and add it in the next free slot
            slots = new List<int>();
            slots.Add(next_free);
            inventoryDictionary.Add(item, slots);
            inventory[next_free].SetItem(item, 1);

            invUIManager.SetSlotItem(next_free, item, 1);

            if (next_free == selected_slot)
            {
                SetupCurrentObject();
            }

            // find next available slot for next time
            FindNextFree();
        }
    }

    public ItemDef GetSelectedSlotsItem()
    {
        return inventory[selected_slot].GetItem();
    }

    public bool IsSelectedSlotEmpty()
    {
        return inventory[selected_slot].IsEmpty();
    }

    public void UseItem()
    {
        if (!inventory[selected_slot].IsEmpty())
        {
            if (currentObjectInterface.Use())
            {
                // tries to remove the item
                bool removed_item = inventory[selected_slot].TryRemove();

                // if it fails then this slot is now empty
                if (!removed_item)
                {
                    List<int> slots;
                    ItemDef item = inventory[selected_slot].GetItem();
                    if (inventoryDictionary.TryGetValue(item, out slots))
                    {
                        // clear the slot
                        inventory[selected_slot].Clear();

                        // set next_free to this slot's position if it is smaller
                        slots.Remove(selected_slot);

                        invUIManager.ClearSlot(selected_slot);

                        if (selected_slot < next_free)
                        {
                            next_free = selected_slot;
                        }

                        currentObject = null;
                        currentObjectInterface = null;

                        // if there are no more of this item left then remove from dictionary
                        if (slots.Count <= 0)
                        {
                            inventoryDictionary.Remove(item);
                        }
                    }
                }
                else
                {
                    invUIManager.SetSlotItemSize(selected_slot, inventory[selected_slot].GetSize());
                    SetupCurrentObject();
                }
            }
        }
    }

    private void FindNextFree()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].IsEmpty())
            {
                next_free = i;
                return;
            }
        }

        next_free = -1;
    }

    public void IncreaseSelectedSlot()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }
        selected_slot = (selected_slot + 1) % columns;
        Debug.Log(selected_slot);
        if (inventory[selected_slot].IsEmpty())
        {
            currentObject = null;
            currentObjectInterface = null;
        }
        else
        {
            SetupCurrentObject();
        }
    }

    public void DecreaseSelectedSlot()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }
        selected_slot = (columns + (selected_slot - 1)) % columns;
        Debug.Log(selected_slot);
        if (inventory[selected_slot].IsEmpty())
        {
            currentObject = null;
            currentObjectInterface = null;
        }
        else
        {
            SetupCurrentObject();
        }
    }

    private void FixedUpdate()
    {
        if (currentObject != null)
        {
            currentObjectInterface.Hover();
        }
    }

    private void SetupCurrentObject()
    {
        currentObject = Instantiate(inventory[selected_slot].GetItem().prefab);
        currentObjectInterface = currentObject.GetComponent<IObject>();
        currentObjectInterface.Activate();
    }
}
