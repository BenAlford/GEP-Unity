using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] GameObject spriteObject;
    Image image;
    ItemDef item;

    private void Awake()
    {
        image = spriteObject.GetComponent<Image>();
        spriteObject.SetActive(false);
        sizeText.SetText("");
    }

    public void SetItem(ItemDef newItem, int size)
    {
        item = newItem;
        image.sprite = item.sprite;
        sizeText.SetText(size.ToString());
        spriteObject.SetActive(true);
    }

    public void SetSize(int size)
    {
        sizeText.SetText(size.ToString());
    }

    public void Hide()
    {
        spriteObject.SetActive(false);
        sizeText.SetText("");
    }
}
