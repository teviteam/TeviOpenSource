using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soils : MonoBehaviour
{

    public GameObject objectSoil;
    public int plantedNeighbours = 0;
    private GenerateSoil soilGenerator;

    private UIManager uiManager;

    public int soilState = 0;
    public Material healthySoilMaterial;
    public Material middleSoilMaterial;
    public Material badSoilMaterial;

    //public List<Soils> emptyNearbySoils;
    public List<Soils> nearbySoil;

    private GameObject temporarySucker;
    private GameObject temporaryCutting;
    private Inventory myInventory;

    public SoilsData mySoilsData;
    public PlantData readOnlyPlantData; // the plant data for the plant planted on this soil SHOULD BE READ ONLY

    // Constructor
    public void SoilsConstructor(int myPosXaxis, int myPosYaxis, GameObject myObjectSoil)
    {
        mySoilsData.posX = myPosXaxis;
        mySoilsData.posY = myPosYaxis;
        objectSoil = myObjectSoil;
    }


    private void Start()
    {
        //#clean this and check if we find before trying to get the component  
        soilGenerator = GameObject.Find("SoilsListAndGeneration").GetComponent<GenerateSoil>();
        //will update the soil state every second
        InvokeRepeating("UpdateSoilState", 0.0f, 0.1f);

        uiManager = GameObject.Find("MyScripts").GetComponent<UIManager>();
        myInventory = GameObject.Find("InventoryObject").GetComponent<Inventory>();

        nearbySoil = new List<Soils>();
    }

    //when clicking on an empty soil, we plant it with the currently selected seed in the inventory
    public void OnMouseUp()
    {
        uiManager.SoilClicked(this);
    }

    //function to update the number of planted neighbour soils
    public void GetSurroundingPlantedSoilsCount()
    {
        if (soilGenerator)
        {
            int neighbourCount = 0;
            //we parse all neighbours but the actual soil tile
            for (int neighbourX = mySoilsData.posX - 1; neighbourX <= mySoilsData.posX + 1; neighbourX++)
            {
                for (int neighbourY = mySoilsData.posY - 1; neighbourY <= mySoilsData.posY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < soilGenerator.numberSoilx && neighbourY >= 0 && neighbourY < soilGenerator.numberSoily)
                    {
                        if (neighbourX != mySoilsData.posX || neighbourY != mySoilsData.posY)
                        {
                            //we have a neighbour, now we search the soils list to find it and find out if it is planted.
                            for (int i = 0; i < soilGenerator.mySoilsList.Count; i++)
                            {
                                if (soilGenerator.mySoilsList[i].name == "soil_box_" + neighbourX + "_" + neighbourY)
                                {
                                    if (soilGenerator.mySoilsList[i].mySoilsData.is_planted)
                                    {
                                        neighbourCount++;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            //we update the number of neighbour for this soil
            plantedNeighbours = neighbourCount;
        }
    }

    //function to ask (very politely) all neighbours and itself to check their neighbouring planted cells count
    public void UpdateAllSurroundingCount()
    {
        if (soilGenerator)
        {
            //we parse all neighbours but the actual soil tile
            for (int neighbourX = mySoilsData.posX - 1; neighbourX <= mySoilsData.posX + 1; neighbourX++)
            {
                for (int neighbourY = mySoilsData.posY - 1; neighbourY <= mySoilsData.posY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < soilGenerator.numberSoilx && neighbourY >= 0 && neighbourY < soilGenerator.numberSoily)
                    {
                        //we have a neighbour, now we search the soils list to find it and update its neighbour count
                        for (int i = 0; i < soilGenerator.mySoilsList.Count; i++)
                        {
                            if (soilGenerator.mySoilsList[i].name == "soil_box_" + neighbourX + "_" + neighbourY)
                            {
                                soilGenerator.mySoilsList[i].GetSurroundingPlantedSoilsCount();
                            }
                        }
                    }
                }
            }
        }
    }

    void UpdateSoilState()
    {
        //Debug.Log("updatesoilstate");
        if (mySoilsData.water_lvl >= 40)
        {
            if (healthySoilMaterial)
            {
                this.GetComponent<Renderer>().material = healthySoilMaterial;
                soilState = 2;
            }
        }
        else if (mySoilsData.water_lvl < 40 && mySoilsData.water_lvl >= 20)
        {
            if (middleSoilMaterial)
            {
                this.GetComponent<Renderer>().material = middleSoilMaterial;
                soilState = 1;
            }
        }
        else
        {
            if (badSoilMaterial)
            {
                this.GetComponent<Renderer>().material = badSoilMaterial;
                soilState = 0;
            }
        }

    }

    //function to generate suckers on the soil of the plant and nearby soils
    public void GenerateSuckers(int mynumberOfSeeds, GameObject myBabySucker, PlantData myplantData)
    {
        if (myBabySucker)
        {
            //Debug.Log("baby sucker ok, we have "+ mynumberOfSeeds);
            if (mynumberOfSeeds == 1)
            {
                //Debug.Log("instantiate 1");
                if (myInventory)
                {
                    CreateSeedHere(this, myBabySucker, myplantData);
                }
            }

            else
            {
                //Debug.Log("instantiate more");
                if (myInventory)
                {
                    for (int i = 0; i < mynumberOfSeeds; i++)
                    {
                        //the first sucker is always on the mother soil
                        if (i == 0)
                        {
                            CreateSeedHere(this, myBabySucker, myplantData);
                        }
                        else if (i > 0)
                        {
                            if (soilGenerator)
                            {
                                //Debug.Log("current soil is soil_box_" + mySoilsData.posX + "_" + mySoilsData.posY);

                                //we parse all neighbours but the actual soil tile to get them all in the list
                                for (int neighbourX = mySoilsData.posX - 1; neighbourX <= mySoilsData.posX + 1; neighbourX++)
                                {
                                    for (int neighbourY = mySoilsData.posY - 1; neighbourY <= mySoilsData.posY + 1; neighbourY++)
                                    {
                                        //verify it is not outside soil limits.
                                        if (neighbourX >= 0 && neighbourX < soilGenerator.numberSoilx && neighbourY >= 0 && neighbourY < soilGenerator.numberSoily)
                                        {
                                            //we have a neighbour, now we search the soils list to find it
                                            for (int j = 0; j < soilGenerator.mySoilsList.Count; j++)
                                            {
                                                if (soilGenerator.mySoilsList[j].name == "soil_box_" + neighbourX + "_" + neighbourY)
                                                {
                                                    nearbySoil.Add(soilGenerator.mySoilsList[j]);
                                                }
                                            }
                                        }
                                    }
                                }

                                //Now we pick as many seeds as we need in the list, minus one that has been planted on the initial soil.
                                int initialNumberOfNeighbours = nearbySoil.Count;
                                mynumberOfSeeds--;
                                for (int k = 0; k < initialNumberOfNeighbours; k++)
                                {
                                    int currentNeighbour = Random.Range(0, nearbySoil.Count);

                                    //if the neighbour soil is empty, we plant a new sucker on it
                                    if (!nearbySoil[currentNeighbour].mySoilsData.has_babyplant && !nearbySoil[currentNeighbour].mySoilsData.is_planted && mynumberOfSeeds > 0)
                                    {
                                        CreateSeedHere(nearbySoil[currentNeighbour], myBabySucker, myplantData);
                                        mynumberOfSeeds--;
                                        nearbySoil.Remove(nearbySoil[currentNeighbour]);
                                        //Debug.Log("We have a randomly picked empty neighbour soil. k= " + k + " mynumberOfSeeds= " + mynumberOfSeeds);
                                    }
                                }

                            }
                        }
                    }
                }

            }
        }
    }

    //function to generate sucker on the soil of the plant only
    public void GenerateInternalBabyPlants(int myNumberOfCuttings, GameObject myBabySucker, PlantData myplantData)
    {
        if (myBabySucker)
        {
            //Debug.Log("baby sucker ok, we have " + myNumberOfCuttings+" internal baby suckers");
            if (myNumberOfCuttings == 1)
            {
                //Debug.Log("instantiate 1");
                if (myInventory)
                {
                    if (myplantData.replantsItself)
                    {
                        //if the plant is able to replants itself from its baby, it will
                        CreateSeedHere(this, myBabySucker, myplantData);
                    }
                    else
                    {
                        //or a seed to pick up is created instead
                        temporaryCutting = Instantiate(myBabySucker, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                        temporaryCutting.transform.SetParent(this.transform);
                        myplantData.myCuttingList.Add(temporaryCutting);
                    }
                }
            }

            else
            {
                //Debug.Log("instantiate more");
                if (myInventory)
                {
                    for (int i = 0; i < myNumberOfCuttings; i++)
                    {
                        //the first sucker is always on the mother soil
                        if (i == 0)
                        {
                            CreateSeedHere(this, myBabySucker, myplantData);
                        }
                        else if (i > 0)
                        {
                            float posX = Random.Range(-0.3f, 0.3f) + this.transform.position.x;
                            float posY = Random.Range(0.0f, 0.5f) + this.transform.position.y;
                            float posZ = Random.Range(-0.3f, 0.3f) + this.transform.position.z;
                            temporaryCutting = Instantiate(myBabySucker, new Vector3(posX, posY, posZ), this.transform.rotation);
                            temporaryCutting.transform.SetParent(this.transform);
                            myplantData.myCuttingList.Add(temporaryCutting);
                        }
                    }
                }
            }
        }
    }

    //fonction to create a seed plant, soon to be sucker
    void CreateSeedHere(Soils mySoil, GameObject myBabySucker, PlantData myplantData)
    {
        if (myInventory)
        {
            if (!mySoil.mySoilsData.has_babyplant)
            {
                //Debug.Log("we are creating a baby sucker on "+ mySoil+ " : " + myplantData);
                temporarySucker = Instantiate(myBabySucker, mySoil.transform.position, mySoil.transform.rotation);
                temporarySucker.transform.SetParent(mySoil.transform);
                //add the new seed to the sucker list
                SeedDatas mySeed = temporarySucker.AddComponent<SeedDatas>();
                if (mySeed)
                {
                    //Debug.Log("seed ok, add to list");
                    float timeBeforeSeedReplant = myplantData.lifetime - myplantData.timeBeforeFruit;
                    mySeed.SeedsConstructor(temporarySucker, mySoil, myplantData.plantPrefabName, timeBeforeSeedReplant);
                    myInventory.SuckersList.Add(mySeed);
                    mySoil.mySoilsData.has_babyplant = true;
                }
            }
        }
    }

    //when loading the game (not a new game), automatically replant
    public void LoadSoil()
    {
        if (mySoilsData.is_planted)
        {
            GameObject myNewPlant = Resources.Load<GameObject>(mySoilsData.plant_name);
            if (myNewPlant)
            {
                //Debug.Log("New plant found for "+ associatedSoil);
                if (myNewPlant.GetComponent<PlantData>() != null)
                {
                    //Debug.Log("Planting on "+ associatedSoil);
                    GameObject.Find("MyScripts").GetComponent<Planting>().PlantingFunction(this.gameObject, myNewPlant, mySoilsData.plant_time);
                }

                mySoilsData.has_babyplant = false;
            }
        }
    }

    //function to verify that we didn't take the last seed of the soil
    public void CheckBabyPlants(Transform mySeed)
    {
        //Debug.Log("checking that there is still a seed on the soil because we removed one");

        bool hasBabySeed = false
            ;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<InventoryItem>())
            {
                InventoryItem myInventoryData= child.gameObject.GetComponent<InventoryItem>();
                //we check that there are seed on the soil and if they are, they are not the seed being destroyed
                if (myInventoryData.itemType == InventoryItem.itemTypes.SEED && child!=mySeed)
                {
                    hasBabySeed = true;
                }
            }
        }

        /*
        InventoryItem[] myInventoryDatas;
        bool hasBabySeed = false;

        myInventoryDatas = GetComponentsInChildren<InventoryItem>();

        foreach (InventoryItem inventoryThing in myInventoryDatas)
        {
            if (inventoryThing.itemType == InventoryItem.itemTypes.SEED && mySeed)
            {
                hasBabySeed = true;
            }
            
        }*/

        if (hasBabySeed == false)
        {
            mySoilsData.has_babyplant = false;
        }
    }

}
