using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : InteractableObject
{
    public ItemData itemData;
    public int amount = 1;

    public override void Interact()
    {
        base.Interact();
        if(InventoryManager.instance != null)
        {
           bool added = InventoryManager.instance.Addltem(itemData, amount);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}
