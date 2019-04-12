using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    Inventory inventory;
    public string itemName = "BLANK NAME";
    public enum itemTypes { TOOL, SEED };
    public itemTypes itemType;
    public Sprite inventoryImage;
    public GameObject associatedPrefab;

    private Soils myParentSoil;

    public void Start()
    {
        inventory = GameObject.Find("InventoryObject").GetComponent<Inventory>();
    }

    public void OnMouseUp()
    {
        //Debug.Log("Picked up " + itemName + ". " + itemName + " added to inventory.");
        //Debug.Log("associated prefab is" + associatedPrefab.name + "item name is " + itemName);
        inventory.AddToInventoryInGame(itemName, 1);
        
        //tell the soil to check if there are still baby plants there if we picked a seed
        if (itemType == itemTypes.SEED)
        {
            myParentSoil=this.transform.parent.GetComponent<Soils>();
            myParentSoil.CheckBabyPlants(this.transform);
        }

        Destroy(gameObject);

    }
}

