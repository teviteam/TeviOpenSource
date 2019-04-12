using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    //Reference to the UI Manager 
    UIManager uiManager;
    public GameObject questTextObj;
    QuestManager questManager;

    //daily shipment of water
    public bool isShipmentInProcess =false;
    private bool isShipmentActive = false;
    private GameObject ConnectionInfosObject;
    private string strLastShipment;
    private System.DateTime dateTimeLastShipment;
    private System.DateTime endTime;

    //A reference the InventoryItem of every item in the game.
    public List<GameObject> allItems = new List<GameObject>();

    //The actual inventory. Stores items the player has at the moment stored as InventoryEntry objects
    public List<InventoryEntry> inventory = new List<InventoryEntry>();

    //The inventory that gets saved and loaded. (the same as the actual inventory but without sprites, prefabs etc) stored as DatabaseInventoryEntrys.
    public List<InventoryData> inventoryDatas = new List<InventoryData>();

    //The currently selected item
    public InventoryEntry currentItem;

    public List<SeedDatas> SuckersList;

    //to generate inventory and store it in the dbb
    public PlayersData inventoryPlayersData;

    //list of all plantnet result for this user
    public List<PlantnetResultData> plantnetResultDatas;

    // Use this for initialization
    void Start()
    {
        uiManager = GameObject.Find("MyScripts").GetComponent<UIManager>();
        questManager = questTextObj.GetComponent<QuestManager>();
        ConnectionInfosObject = GameObject.Find("ConnectionInfos");
        //AddInitialItems(); //Adds the initial items to the inventory and creates menu tiles for them

    }

    //called by connecting script once the game is fully loaded 
    //and after the final quest of the tuto has been completed
    public void BeginShipmentCheck()
    {         
        strLastShipment = ConnectionInfosObject.GetComponent<ConnectionInfo>().myPlayersData.playerLastShipment;
        Debug.Log("strLastShipment :" + strLastShipment);

        //if the player has finished the quests, she should have a proper date
        if (strLastShipment != "0000-00-00 00:00:00")
        {
            dateTimeLastShipment = System.DateTime.Parse(strLastShipment);
            Debug.Log("dateTimeLastShipment :" + dateTimeLastShipment);
            isShipmentActive = true;
            endTime = dateTimeLastShipment.AddHours(24);//21 à 18h => 22 à 18h
            Debug.Log("endtime:" + endTime);
        }
    }

    private void Update()
    {
        //check the timer of the daily shipment if the player has completed the requested quests and is not in shipment now
        if (isShipmentActive && !isShipmentInProcess) 
        {
            //Debug.Log("in the loop");

            System.DateTime nowTime = System.DateTime.Now;//22 à 19h 
            //Debug.Log("nowTime:" + nowTime);
            System.TimeSpan span = nowTime.Subtract(endTime);// when we passed 24h, it becames positive

            //Debug.Log("minutes since last shipment :"+span.Minutes+" just span :"+ span);

            if (span.Minutes > 0)
            {
                //#todo: window to warn about the shipment
                Debug.Log("shipment is on!!!");
                isShipmentInProcess = true;
                AddToInventoryInGame("WateringCan", 20);
                ConnectionInfosObject.GetComponent<Connecting>().StartSetShipment();//reset shipment to now
            }
        }
    }

    //Runs through inventoryDatas and populates the full inventory. inventoryDatas must have something in it first!
    public void LoadInventory()
    {
        inventory.Clear();

        if (inventoryDatas.Count > 0)
        {
            for (int i = 0; i < inventoryDatas.Count; i++) //for each inventory data
            {
                for (int ii = 0; ii < allItems.Count; ii++) //for each item
                {
                    if (allItems[ii].GetComponent<InventoryItem>()) //so long as the item has an InventoryItem component (all of them should)
                    {
                        InventoryItem item = allItems[ii].GetComponent<InventoryItem>(); //get the InventoryItem component
                        
                        if (inventoryDatas[i].itemName == item.name) //if the InventoryData name matches the InventoryItem name
                        {
                            //Debug.Log(item.name + " : " + inventoryDatas[i].itemName);
                            AddToInventory(item,inventoryDatas[i].itemCount); //Create an InventoryEntry for that item, set the number equal to the InventoryData.itemCount
                        }
                    }
                }
            }
        }
    } 


    private InventoryEntry InventoryItemToInventoryEntry(InventoryItem item)
    {
        if (item != null)
        {
            InventoryEntry entry = new InventoryEntry();

            entry.itemName = item.itemName;
            entry.itemType = item.itemType;
            entry.inventoryImage = item.inventoryImage;
            entry.associatedPrefab = item.associatedPrefab;

            return entry;
        }
        return null;
    }

    public void AddToInventory(InventoryItem inv, int amount)
    {
        //Debug.Log("Adding to the inventory version 2");
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == inv.itemName) //if the inventory contains one of these items already...
            {
                Debug.Log("Just updating");
                inventory[i].itemCount += amount; //increase the count in the inventory

                uiManager.UpdateMenuTileNumber(inv.itemName, inventory[i].itemCount); //search for and update the relevant MenuTile
                return; //return so we don't run on into the next part fo the method.
            }
        }

        // if this item is not already in the inventory...
        inventory.Add(InventoryItemToInventoryEntry(inv)); //add it

        for (int i = 0; i < inventory.Count; i++) //now we should be able to find one
        {
            if (inventory[i].itemName == inv.itemName)
            {
                //Debug.Log("Actually creating");
                inventory[i].itemCount += amount; //increase the count by the specified amount

                //create a new inventory instance in the list of InventoryDatas
                //InventoryData temp = new InventoryData(inventoryPlayersData.playerId, inventory[i].itemName, inventory[i].itemCount);
                //inventoryDatas.Add(temp);
            }

        }
        uiManager.UpdateMenu(); //then rebuild the menu, creating a new menutile etc for it.
    }


    public void AddToInventoryInGame(string gameObjectName, int amount)
    {
        //Debug.Log(gameObjectName);

        if (gameObjectName != null)
        {
            questManager.getItemQuestCheck(gameObjectName, amount);
            foreach (var GameObject in allItems)
            {
                if (GameObject.name == gameObjectName)
                {
                    //Debug.Log(GameObject.name + " " + gameObjectName);

                    if (GameObject.GetComponent<InventoryItem>() != null)
                    {
                        InventoryItem newItem = GameObject.GetComponent<InventoryItem>();

                        //Debug.Log("Adding to the inventory in game");
                        for (int i = 0; i < inventory.Count; i++)
                        {
                            if (inventory[i].itemName == newItem.itemName) //if the inventory contains one of these items alreadywhich it should do as the inventory should contain an entry for each item in the game
                            {
                                if (inventory[i].itemCount > 0)
                                {
                                    //Debug.Log("incrementing count");
                                    inventory[i].itemCount += amount; //increase the count in the inventory
                                    inventoryDatas[i].itemCount += amount;

                                    uiManager.UpdateMenuTileNumber(newItem.itemName, inventory[i].itemCount); //search for and update the relevant MenuTile

                                }
                                else
                                {
                                    //Debug.Log("incrementing count");
                                    inventory[i].itemCount += amount; //increase the count in the inventory
                                    inventoryDatas[i].itemCount += amount;
                                    uiManager.UpdateMenu();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void RemoveFromInventory(string itemName)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == itemName)
            {
                questManager.useItemQuestCheck(itemName);

                if ((inventory[i].itemCount - 1) <= 0) //if removing this item would mean there are none left in the menu
                {
                    //Debug.Log("Removing the final " + inventory[i].itemName + " from inventory.");
                    inventory[i].itemCount--;
                    inventoryDatas[i].itemCount--;
                    //inventory.Remove(inventory[i]); //remove the item
                    uiManager.UpdateMenu(); //rebuild the menu
                    currentItem = null; //remove the item from the 'currently selected' slot
                    return;
                }

                //else simply decrement the value for the particular item
                //Debug.Log("Removing 1 " + inventory[i].itemName + " from inventory.");
                inventory[i].itemCount--;
                inventoryDatas[i].itemCount--;
                uiManager.UpdateMenuTileNumber(itemName, inventory[i].itemCount);
            }
        }
    }
}
