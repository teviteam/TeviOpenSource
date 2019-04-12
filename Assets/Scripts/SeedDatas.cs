using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script attached to the seeds : before the plant begins to grow, can be gathered before they are planted
public class SeedDatas : MonoBehaviour {

    public GameObject seed;
    public Soils associatedSoil;
    public string plantPrefabName;
    public float timeToPlant;

    private float initialTimer;
    private float seedTimer;

    private GameObject inventoryObj;
    private Inventory inventory;

    // Seed Constructor
    public void SeedsConstructor(GameObject mySeed, Soils myAssociatedSoil, string myPlantPrefabName, float myTimeToPlan)
    {
        seed = mySeed;
        associatedSoil = myAssociatedSoil;
        plantPrefabName = myPlantPrefabName;
        timeToPlant = myTimeToPlan;
    }

    //seed growth
    void Start()
    {
        //Debug.Log("Beginning of seed growth for " + associatedSoil);
        initialTimer = Time.time;
        seedTimer = 0;

        //inventory link
        inventoryObj = GameObject.Find("InventoryObject");
        if (inventoryObj)
        {
            inventory = inventoryObj.GetComponent<Inventory>();
        }
    }

    void Update()
    {
        seedTimer = Time.time - initialTimer;

        if (seed && associatedSoil && plantPrefabName!="")
        {
            if (seedTimer > timeToPlant && associatedSoil.mySoilsData.has_babyplant == true)
            {
                //Debug.Log("Seed will be replanted");

                // Debug.Log("replant the baby sucker of the same type as the plant : " + plantPrefabName);

                GameObject myNewPlant = Resources.Load<GameObject>(plantPrefabName);
                if (myNewPlant)
                {
                    //Debug.Log("New plant found for "+ associatedSoil);
                    if (myNewPlant.GetComponent<PlantData>() != null && !associatedSoil.mySoilsData.is_planted)
                    {
                        //Debug.Log("Planting on "+ associatedSoil);
                        associatedSoil.mySoilsData.is_planted = true;
                        GameObject.Find("MyScripts").GetComponent<Planting>().PlantingFunction(associatedSoil.gameObject, myNewPlant,0);
                    }

                    associatedSoil.mySoilsData.has_babyplant = false;
                    foreach (Transform child in associatedSoil.transform)
                    {
                        if (child.name != "Twinkle(Clone)")
                        {
                            inventory.SuckersList.Remove(child.GetComponent<SeedDatas>());
                            GameObject.Destroy(child.gameObject);
                        }

                       
                    }
                }
            }
        }
    }


}
