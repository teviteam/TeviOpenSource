using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantData : MonoBehaviour {

    //speed of the all game, change with caution, the highest the slowest
    private float gameSpeed = 0.2f;

    [Header("Generic data to fill")]
    public string plantName = "";
    public string plantPrefabName = "";
    public float timeBeforeMature = 2.0f;
    public float timeBeforeFruit = 3.0f;
    public float timeToGatherFruit = 3.0f;
    public float timeBeforeDeath = 2.0f;
    public int fruitCycles = 1;
    public float averageHeight = 1.0f;//actually the maximum height
    public float averageSize = 0.5f;

    private float heightGrowthPerCycle = 0.1f;

    [Header("Prefab to add")]
    public GameObject matureForm;
    public GameObject fruit;
    public GameObject babySucker;

    [Header("Materials to add")]
    public Material healthyPlantMaterial;
    public Material mediumPlantMaterial;
    public Material badPlantMaterial;

    [Header("Amount of Water, Nutrient in soil and Neighbours for the plant to be happy")]
    public int miniHappyWater = 0;
    public int maxHappyWater = 50;
    public int miniHappyNutrient = 0;
    public int maxHappyNutrient = 50;
    public int miniHappyNeigbours = 0;
    public int maxHappyNeigbours = 8;

    [Header("Water and Nutrient consumption while growing")]
    //how much water is consumer over each phase of plant life according to its state
    public int happyWaterConsumption = 0;
    public int okWaterConsumption = 0;
    public int badWaterConsumption = 0;

    //same for nutrients
    public int happyNutrientConsumption = 0;
    public int okNutrientConsumption = 0;
    public int badNutrientConsumption = 0;

    [Header("Reproduction data")]
    public int miniFruitOk = 0;
    public int maxFruitOk = 3;
    public int miniFruitHappy = 2;
    public int maxFruitHappy = 7;
    public int miniSeedOk = 1;
    public int maxSeedOk = 3;
    public int miniSeedHappy = 2;
    public int maxSeedHappy = 5;
    public int miniCuttingOk = 1;
    public int maxCuttingOk = 3;
    public int miniCuttingHappy = 2;
    public int maxCuttingHappy = 5;
    public bool replantsItself = false;
    private int numberOfFruits = 0;
    private int numberOfSeeds = 0;
    private int numberOfCuttings = 0;

    [Header("not to be modified")]
    public int plantHappiness; //0-1-2 levels of plant happyness (0 not happy, 1 ok, 2 happy) Should not be modified manually
    public GameObject myMatureForm; // Should not be modified manually
    public bool isMature = false;
    public bool isFruit = false;
    private GameObject temporaryFruit;
    public List<GameObject> myFruitsList;
    public List<GameObject> myCuttingList;
    public float lifetime = 90.0f;

    public void SetSpeed()
    {
        timeBeforeMature = timeBeforeMature / gameSpeed;
        timeBeforeFruit = timeBeforeFruit / gameSpeed;
        timeToGatherFruit = timeToGatherFruit / gameSpeed;
        timeBeforeDeath = timeBeforeDeath/gameSpeed;
    }

    //function that displays the mature (bigger) form of a plant, if it is happy enough, called by growth 
    public void DisplayMature(int currentCycle) {

        heightGrowthPerCycle = averageHeight/fruitCycles/2;
            //Debug.Log("display mature form of the plant, cycle "+ currentCycle);

        if(!myMatureForm)
        {
            //instantiate and attach mature form to the seed
            myMatureForm = Instantiate(matureForm, this.transform.position, this.transform.rotation);
            myMatureForm.transform.SetParent(this.transform);
            //hide seed
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
        /* if(currentCycle==1)
        {
            //instantiate and attach mature form to the seed
            myMatureForm = Instantiate(matureForm, this.transform.position, this.transform.rotation);
            myMatureForm.transform.SetParent(this.transform);
            //hide seed
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }*/

        myMatureForm.transform.localScale = new Vector3(averageSize / this.transform.localScale.x, averageHeight/2+ heightGrowthPerCycle * currentCycle / this.transform.localScale.y, averageSize / this.transform.localScale.z);

    }

    //function that generates fruits and seeds from a plant if it is happy enough to reproduce, called by growth 
    public void DisplayReproduction(int plantHappinessReproduction, Soils soil, int currentCycle){

        //hide mature form if it is the last cycle
        if(currentCycle== fruitCycles)
        {
            if(myMatureForm)
            {
                myMatureForm.GetComponent<Renderer>().enabled = false;
            }           
        }       

        //Debug.Log("display fruits and seeds");
        //very efficient reproduction phase
        if(plantHappinessReproduction==2)
        {
            numberOfFruits = Random.Range(miniFruitHappy, maxFruitHappy);
            numberOfSeeds = Random.Range(miniSeedHappy, maxSeedHappy);
            numberOfCuttings = Random.Range(miniCuttingHappy, maxCuttingHappy);
        }
        else if (plantHappinessReproduction == 1)
        {
            numberOfFruits = Random.Range(miniFruitOk, maxFruitOk);
            numberOfSeeds = Random.Range(miniSeedOk, maxSeedOk);
            numberOfCuttings = Random.Range(miniCuttingHappy, maxCuttingHappy);
        }

        //generation of fruits
        if (numberOfFruits!=0)
        {
            //Debug.Log("we have produced "+ numberOfFruits + " fruits");
            if (fruit)
            {
                for (int i = 0; i < numberOfFruits; i++)
                {
                    float posX = Random.Range(-0.3f, 0.3f) + this.transform.position.x;
                    float posY = Random.Range(0.0f, 0.5f) + this.transform.position.y;
                    float posZ = Random.Range(-0.3f, 0.3f) + this.transform.position.z;
                    temporaryFruit = Instantiate(fruit, new Vector3(posX, posY, posZ), this.transform.rotation);
                    temporaryFruit.transform.SetParent(this.transform);
                    myFruitsList.Add(temporaryFruit);
                }
            }
        }

        //generation of suckers (for plants that produce them)
        if (numberOfSeeds != 0)
        {
            //Debug.Log("we have produced " + numberOfSeeds + " suckers");
            soil.GenerateSuckers(numberOfSeeds,babySucker,this);
        }

        //generation of Internal suckers (for plants that produce them)
        if (numberOfCuttings != 0)
        {
            //Debug.Log("we have produced " + numberOfCuttings + " internal suckers");
            soil.GenerateInternalBabyPlants(numberOfCuttings, babySucker, this);
        }
    }

}
