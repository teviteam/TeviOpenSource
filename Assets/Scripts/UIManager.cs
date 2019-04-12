using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //This class keeps track of what button is currently selected, and handles what happens when something is clicked (at the moment just the soil)

    public enum UIButtons { Planting, Watering, AddingNutrients };
    public UIButtons currentButton;

    private GameObject inventoryObj;
    private Inventory inventory;

    //things required for the Menubar and Menutiles
    private GameObject menuBar;
    public GameObject menuBackground;
    private RectTransform menuBarRect;
    private RectTransform menuBackgroundRect;
    
    public List<GameObject> menuTiles = new List<GameObject>();
    public float inventoryTileSize = 200.0f;
    public float inventoryMargin = 20.0f;

    private int amountOfWaterToAdd = 5;
    private int amountOfNutrientToAdd = 5;

    private GameObject connectionObj;
    private ConnectionInfo connectionInstance;

    //things required for the infoPopup
    public Camera mainCamera;
    public GameObject infoPopup; //reference to the infoPopup prefab
    private Soils inspectingThisSoil; //the soil the infoPopup is currently looking at
    public Image face;
    public Sprite happyFace;
    public Sprite mediumFace;
    public Sprite sadFace;
    public Image neighbourIcon;
    public Sprite tooManyNeighbour;
    public Sprite idealNeighbour;
    public Sprite tooFewNeighbour;
    public Image waterIcon;
    public Sprite tooMuchWater;
    public Sprite idealWater;
    public Sprite tooLitteWater;
    public Image nutrientIcon;
    public Sprite tooManyNutrient;
    public Sprite idealNutrient;
    public Sprite tooFewNutrient;

    public bool doNotPlant = false; //flag to prevent planting if mouse is over UI

    //ADDED BY GIOVANNI: reference to quest manager
    public GameObject questTextObj;
    QuestManager questManager;
    //ADDED BY GIOVANNI

    // Use this for initialization
    void Start()
    {
        menuTiles = new List<GameObject>();
        menuBar = GameObject.Find("MenuBar");
        if (menuBar != null)
        {
            menuBarRect = menuBar.GetComponent<RectTransform>();
        }

        menuBackground = GameObject.Find("MenuBackground");
        if (menuBackground != null)
        {
            menuBackgroundRect = menuBackground.GetComponent<RectTransform>();
        }

        inventoryObj = GameObject.Find("InventoryObject");
        if (inventoryObj)
        {
            inventory = inventoryObj.GetComponent<Inventory>();
        }

        connectionObj = GameObject.Find("ConnectionInfos");
        if (connectionObj)
        {
            connectionInstance = connectionObj.GetComponent<ConnectionInfo>();
            SayHello();
        }

        //ADDED BY GIOVANNI: reference to quest manager
        questManager = questTextObj.GetComponent<QuestManager>();
        //ADDED BY GIOVANNI
    }

    public void SoilClicked(Soils soil)
    {
        if (inventory)
        {
            if (inventory.currentItem != null && doNotPlant == false) //changed from if(inventory.currentItem != null) which evaluated to 'false' even when there was an item there
            {
                switch (inventory.currentItem.itemType)
                {
                    case InventoryItem.itemTypes.SEED:

                        if (inventory.currentItem.associatedPrefab != null)
                        {
                            if (inventory.currentItem.associatedPrefab.GetComponent<PlantData>() != null && !soil.mySoilsData.is_planted)
                            {
                                soil.mySoilsData.is_planted = true;
                                GameObject.Find("MyScripts").GetComponent<Planting>().PlantingFunction(soil.gameObject, inventory.currentItem.associatedPrefab, 0);
                                inventory.RemoveFromInventory(inventory.currentItem.itemName);
                            }

                        }
                        break;

                    case InventoryItem.itemTypes.TOOL:

                        if (inventory.currentItem.itemName == "WateringCan" && soil.mySoilsData.water_lvl < 50)
                        {
                            soil.mySoilsData.water_lvl += amountOfWaterToAdd;

                            inventory.RemoveFromInventory(inventory.currentItem.itemName);

                            return; //we must return after each of these cases to avoid crashing when the last object is removed from the inventory
                        }

                        if (inventory.currentItem.itemName == "Fertilizer" && soil.mySoilsData.nutrient_lvl < 50)
                        {
                            //Debug.Log("before soil.nutrientInSoil:" + soil.nutrientInSoil+ " amountOfNutrientToAdd:" + amountOfNutrientToAdd);
                            soil.mySoilsData.nutrient_lvl += amountOfNutrientToAdd;
                            //Debug.Log("after nutrient" + soil.nutrientInSoil);
                            inventory.RemoveFromInventory(inventory.currentItem.itemName);
                            return;
                        }
                        if (inventory.currentItem.itemName == "Inspector" && soil.mySoilsData.nutrient_lvl < 50)
                        {
                            if (infoPopup != null && mainCamera != null && soil.readOnlyPlantData != null) //if we have the infoPopup reference, the main camera reference, and the soil clicked has a plant on it
                            {
                                if (infoPopup.activeInHierarchy == true && inspectingThisSoil == soil)
                                {
                                    infoPopup.SetActive(false);
                                    return;
                                }

                                if (infoPopup.activeInHierarchy == false)
                                {
                                    //ADDED BY GIOVANNI: it calls function in questManager to set the inspector quest as completed
                                    questManager.inspectorQuest();
                                    //ADDED BY GIOVANNI

                                    infoPopup.SetActive(true);
                                }

                                inspectingThisSoil = soil;                                           



                            }
                            return;
                        }
                        break;

                }
            }
        }
    }

    // keep the infoPopup on top of the right soil, for when the player drags the map
    public void UpdatePopupPosition()
    {


    }

    public void PlantingSelected()
    {
        currentButton = UIButtons.Planting;
    }

    public void WateringSelected()
    {
        currentButton = UIButtons.Watering;
    }

    public void AddNutrientsSelected()
    {
        currentButton = UIButtons.AddingNutrients;
    }

    public void PopulateMenu()
    {

        if (menuBar && (inventory.inventory.Count > 0))
        {
            RectTransform rt = menuBar.GetComponent<RectTransform>();

            float menuHeight = rt.rect.height;

            for (int i = 0; i < inventory.inventory.Count; i++)
            {

                if (inventory.inventory[i].itemCount > 0)
                {
                    GameObject go = (GameObject)Instantiate(Resources.Load("MenuTile"));

                    go.transform.SetParent(menuBar.transform);

                    //the pivot is set on the bottom left
                    go.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 0.0f);
                    go.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.0f);
                    go.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 0.0f);

                    //scale and size of the tile are initialized
                    go.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(inventoryTileSize, inventoryTileSize);

                    float posY = menuHeight / 2 - inventoryTileSize / 2;//centers the tile vertically
                    float posX = inventoryMargin + menuTiles.Count * (inventoryTileSize + inventoryMargin);

                    go.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, 0.0f);

                    go.transform.Find("Image").gameObject.GetComponent<Image>().sprite = inventory.inventory[i].inventoryImage;
                   // go.GetComponent<Image>().sprite = inventory.inventory[i].inventoryImage;

                    go.GetComponent<MenuTile>().inventoryEntry = inventory.inventory[i]; //give the menu tile a reference to the inventory item (for when it's clicked)

                    go.GetComponentInChildren<Text>().text = inventory.inventory[i].itemCount.ToString(); //display the number of these items on the menu tile.

                    menuTiles.Add(go); //Add the tile to the list of tiles so we can keep track of it.
                }

            }
            rt.sizeDelta = new Vector2(inventoryMargin + menuTiles.Count * (inventoryTileSize + inventoryMargin), inventoryTileSize);

            //stretch the menubar background to the size of the current menu
            if (menuBar != null && menuBackground != null)
            {
                menuBackgroundRect.sizeDelta = new Vector2 (80 + menuBarRect.sizeDelta.x, menuBackgroundRect.sizeDelta.y);
            }
        }
    }

    public void UpdateMenu()
    {
        for (int i = 0; i < menuTiles.Count; i++)
        {
            Destroy(menuTiles[i].gameObject);
        }
        menuTiles.Clear();
        PopulateMenu();
    }

    public void UpdateMenuTileNumber(string itemName, int number)
    {

        for (int i = 0; i < menuTiles.Count; i++)
        {
            if (menuTiles[i].GetComponent<MenuTile>().inventoryEntry.itemName == itemName)
            {
                menuTiles[i].GetComponent<MenuTile>().inventoryEntry.itemCount = number;
                menuTiles[i].GetComponentInChildren<Text>().text = number.ToString();
            }
        }
    }

    //This function if called by the MenuTile when one is clicked, it removes the highlighting on all tiles and adds highlighting on the selected one.
    public void ColorSelectedMenuTile(MenuTile selectedTile)
    {

        for (int i = 0; i < menuTiles.Count; i++)
        {
            MenuTile currentTile = menuTiles[i].GetComponent<MenuTile>();
            if (currentTile)
            {
                currentTile.UnSelect();
            }
        }

        if (inventory.currentItem == selectedTile.inventoryEntry && inventory.currentItem.itemName != "BananaFruit" && inventory.currentItem.itemName != "BreadfruitFruit" && inventory.currentItem.itemName != "ShampooGingerFruit" && inventory.currentItem.itemName != "SugarcaneFruit" && inventory.currentItem.itemName != "WildindigoFruit")
        {
            selectedTile.Select();
        }
    }

    public void SayHello()
    {
        if (GameObject.Find("TitleText"))
        {
            GameObject.Find("TitleText").GetComponent<Text>().text = "Welcome to your outpost, dear " + connectionInstance.myPlayersData.playerUsername;
        }
    }

    void Update()
    {
        //if the plant being inspected dies, the inspector popup dissapears
        if (infoPopup.activeInHierarchy == true && inspectingThisSoil.readOnlyPlantData == null)
        {
            infoPopup.SetActive(false);
        }

        if (infoPopup.activeInHierarchy == true)
        {
            Vector3 v3 = mainCamera.WorldToScreenPoint(inspectingThisSoil.transform.position);
            v3.y += 75;
            infoPopup.transform.position = v3;

            //Debug.Log(soil.name);
            //Debug.Log("planted neighbours: " + soil.plantedNeighbours + " " + "happy neighbours: " + soil.readOnlyPlantData.maxHappyNeigbours);
            //Debug.Log("water lvl: " + soil.mySoilsData.water_lvl + " " + "happy water: " + soil.readOnlyPlantData.maxHappyWater);
            //Debug.Log("nutrient lvl: " + soil.mySoilsData.nutrient_lvl + " " + "happy nutrients: " + soil.readOnlyPlantData.maxHappyNutrient);

            //Water
            if (inspectingThisSoil.mySoilsData.water_lvl > inspectingThisSoil.readOnlyPlantData.maxHappyWater)
            {
                if (tooMuchWater != null)
                {
                    waterIcon.sprite = tooMuchWater;
                }
            }

            if (inspectingThisSoil.mySoilsData.water_lvl <= inspectingThisSoil.readOnlyPlantData.maxHappyWater && inspectingThisSoil.mySoilsData.water_lvl >= inspectingThisSoil.readOnlyPlantData.miniHappyWater) //the +10 is here to match the way the plant happines is calculated in the Growth script
            {
                if (idealWater != null)
                {
                    waterIcon.sprite = idealWater;
                }

            }

            if (inspectingThisSoil.mySoilsData.water_lvl < inspectingThisSoil.readOnlyPlantData.miniHappyWater)
            {
                if (tooLitteWater != null)
                {
                    waterIcon.sprite = tooLitteWater;
                }
            }

            //Neighbours
            if (inspectingThisSoil.plantedNeighbours > inspectingThisSoil.readOnlyPlantData.maxHappyNeigbours)
            {
                if (tooManyNeighbour != null)
                {
                    neighbourIcon.sprite = tooManyNeighbour;
                }
            }

            if (inspectingThisSoil.plantedNeighbours <= inspectingThisSoil.readOnlyPlantData.maxHappyNeigbours && inspectingThisSoil.plantedNeighbours >= inspectingThisSoil.readOnlyPlantData.miniHappyNeigbours) //the +10 is here to match the way the plant happines is calculated in the Growth script
            {
                if (idealNeighbour != null)
                {
                    neighbourIcon.sprite = idealNeighbour;
                }

            }

            if (inspectingThisSoil.plantedNeighbours < inspectingThisSoil.readOnlyPlantData.miniHappyNeigbours)
            {
                if (tooFewNeighbour != null)
                {
                    neighbourIcon.sprite = tooFewNeighbour;
                }
            }

            //Nutrients
            if (inspectingThisSoil.mySoilsData.nutrient_lvl > inspectingThisSoil.readOnlyPlantData.maxHappyNutrient)
            {
                if (tooManyNutrient != null)
                {
                    nutrientIcon.sprite = tooManyNutrient;
                }
            }

            if (inspectingThisSoil.mySoilsData.nutrient_lvl <= inspectingThisSoil.readOnlyPlantData.maxHappyNutrient && inspectingThisSoil.mySoilsData.nutrient_lvl >= inspectingThisSoil.readOnlyPlantData.miniHappyNutrient) //the +10 is here to match the way the plant happines is calculated in the Growth script
            {
                if (idealNutrient != null)
                {
                    nutrientIcon.sprite = idealNutrient;
                }
            }

            if (inspectingThisSoil.mySoilsData.nutrient_lvl < inspectingThisSoil.readOnlyPlantData.miniHappyNutrient)
            {
                if (tooFewNutrient != null)
                {
                    nutrientIcon.sprite = tooFewNutrient;
                }
            }


            //face colour
            if (inspectingThisSoil.readOnlyPlantData.plantHappiness==0 )
            {
                if (sadFace != null)
                {
                    face.sprite = sadFace;
                }
            }
            if (inspectingThisSoil.readOnlyPlantData.plantHappiness == 1)
            {
                if (mediumFace != null)
                {
                    face.sprite = mediumFace;
                }
            }
            if (inspectingThisSoil.readOnlyPlantData.plantHappiness == 2)
            {
                if (happyFace != null)
                {
                    face.sprite = happyFace;
                }
            }
        }
    }
}