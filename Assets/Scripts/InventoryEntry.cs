using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEntry 
{
    public string itemName = "BLANK NAME";

    public int itemCount;

    public InventoryItem.itemTypes itemType;

    public GameObject associatedPrefab;

    public Sprite inventoryImage;

    public int id_inventory;
    public int player_id;
}
