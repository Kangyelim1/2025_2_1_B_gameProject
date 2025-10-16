using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.UI;

public class InventorSlot : MonoBehaviour
{
    public ItemData item;
    public int amont;

    [Header("UI Refernece")]
    public Image itemlcon;
    public Text amontText;
    public GameObject emptySlotImage;

    void Start()
    {
        UpdateSlotUl();
    }

    public void Setltem(ItemData newItem, int newAmont)
    {
        item = newItem;
        amont = newAmont;
        UpdateSlotUl();
    }

    void UpdateSlotUl()
    {
        if (item != null)
        {
            itemlcon.sprite = item.itemIcon;
            itemlcon.enabled = true;

            amontText.text = amont > 1 ? amont.ToString() : "";
            if(emptySlotImage != null)
            {
                emptySlotImage.SetActive(false);
            }
        }
        else
        {
            itemlcon.enabled = false;
            amontText.text = "";

            if (emptySlotImage != null)
            {
                emptySlotImage.SetActive(true);
            }
        }
    }

    public void AddAmount(int value)
    {
        amont += value;
        UpdateSlotUl();
    }

    public void RemoveAmount(int value)
    {
        amont -= value;
        if(amont <= 0)
        {
            ClearSlot();
        }
        else
        {
            UpdateSlotUl();
        }
    }

    public void ClearSlot()
    {
        item = null;
        amont = 0;
        UpdateSlotUl();
    }
}