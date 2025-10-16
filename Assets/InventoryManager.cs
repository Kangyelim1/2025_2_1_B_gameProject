using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory Setup")]
    public int inventorySize = 20;
    public GameObject inventroyUI;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;
    public List<InventorSlot> slots = new List<InventorSlot>();
    public bool islnventoryOpen = false;

   private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        CreateInventorySlots();
        inventroyUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(inventoryKey))
        {
            ToggLeInventory();
        }
    }

    void CreateInventorySlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotParent);
            InventorSlot slot = slotObject.GetComponent<InventorSlot>();
            slots.Add(slot);
        }
    }

    public void ToggLeInventory()
    {
        islnventoryOpen = !islnventoryOpen;
        inventroyUI.SetActive(islnventoryOpen);
        
        if(islnventoryOpen)
        {
           Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool Addltem(ItemData item, int amount = 1)
    {
        foreach (InventorSlot slot in slots)
        {
            if(slot.item == item && slot.amont < item.maxStack)
            {
               int spaceLeft = item.maxStack - slot.amont;
                int amountToAdd = Mathf.Min(amount, spaceLeft);
                slot.AddAmount(amountToAdd);
                amount -= amountToAdd;
                if (amount <= 0)
                {
                    return true;
                }
            }
        }
        foreach (InventorSlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.Setltem(item, amount);
                return true;
            }
        }
        Debug.Log("인벤토리가 가둑");
        return false;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        foreach(InventorSlot slot in slots)
        {
            if (slot.item == item)
            {
               slot.RemoveAmount(amount);
                return;

            }
        }
    }

    public int GetItemCount(ItemData item)
    {
       int count = 0;
        foreach (InventorSlot slot in slots)
        {
            if (slot.item == item)
            {
                count += slot.amont;
            }
        }
        return count;
    }


}
