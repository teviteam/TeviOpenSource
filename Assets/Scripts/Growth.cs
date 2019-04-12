using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script managing the growth of plants, one they have been planted after seed growth

public class Growth : MonoBehaviour {

    //scripting variables
    private float initialTimer=0;
    private PlantData myPlantData;
    public GameObject SoilOfThePlant;
    private Soils mySoils;
    private int currentCycle=1;//should never be 0 as it is used to divide
    private float initialPlantSize;
    private bool startDone=false;
    
    // Use this for initialization
    void Start () {

        //initialTimer = Time.time;

        //get plant data
        myPlantData = this.gameObject.GetComponent<PlantData>();
        myPlantData.SetSpeed();

        initialPlantSize = myPlantData.gameObject.transform.localScale.x;
                
        //get soil data
        if (SoilOfThePlant)
        {
            mySoils = SoilOfThePlant.GetComponent<Soils>();
            initialTimer = Time.time- mySoils.mySoilsData.plant_time;
           
        }
        //Debug.Log("mySoils.mySoilsData.plant_time:" + mySoils.mySoilsData.plant_time);
        //Debug.Log("myPlantData.timeBeforeMature:" + myPlantData.timeBeforeMature+ " myPlantData.timeBeforeFruit:" + myPlantData.timeBeforeFruit + " myPlantData.timeToGatherFruit:" + myPlantData.timeToGatherFruit + " myPlantData.timeBeforeDeath:" + myPlantData.timeBeforeDeath+ " myPlantData.fruitCycles:"+ myPlantData.fruitCycles);

        if (myPlantData && mySoils.mySoilsData.plant_time==0)
        {
            currentCycle = 1;
            //# this should be improved
            //myPlantData.lifetime = ((myPlantData.timeBeforeMature + myPlantData.timeBeforeFruit + myPlantData.timeToGatherFruit) * myPlantData.fruitCycles + myPlantData.timeBeforeDeath)- mySoils.mySoilsData.plant_time;
            myPlantData.lifetime = ((myPlantData.timeBeforeMature + myPlantData.timeBeforeFruit + myPlantData.timeToGatherFruit) * myPlantData.fruitCycles + myPlantData.timeBeforeDeath);
            mySoils.mySoilsData.plant_name = myPlantData.plantPrefabName;
        }
        else if(myPlantData && mySoils.mySoilsData.plant_time > 0)
        {
            myPlantData.lifetime = ((myPlantData.timeBeforeMature + myPlantData.timeBeforeFruit + myPlantData.timeToGatherFruit) * myPlantData.fruitCycles + myPlantData.timeBeforeDeath);
            mySoils.mySoilsData.plant_name = myPlantData.plantPrefabName;
        }
        else
        {
            Debug.Log("No plant data found for " + this.gameObject.name);
        }
        startDone = true;
        //update plant happiness state and related appearance
        InvokeRepeating("UpdatePlantAppearanceHappiness", 0.0f, 1.0f);

        //Debug.Log("current time:" + Time.time + " initialTimer:" + initialTimer+ " myPlantData.lifetime:" + myPlantData.lifetime);

    }
	
	void Update () {
        if (startDone)
        {
            //checking major state of the plant growth #problem here, to be improved
            mySoils.mySoilsData.plant_time = Time.time - initialTimer;
            //Debug.Log("mySoils.mySoilsData.plant_time = " + mySoils.mySoilsData.plant_time);

            //cycle management, especially interesting for plant with several life cycles
            if (mySoils.mySoilsData.plant_time > (myPlantData.timeBeforeMature + myPlantData.timeBeforeFruit + myPlantData.timeToGatherFruit) * currentCycle && mySoils.mySoilsData.plant_time < (myPlantData.timeBeforeMature + myPlantData.timeBeforeFruit + myPlantData.timeToGatherFruit) * myPlantData.fruitCycles)
            {
                currentCycle++;
                Debug.Log("On to cycle" + currentCycle);
                //Debug.Log("mySoils.mySoilsData.plant_time:" + mySoils.mySoilsData.plant_time+", next growth time "+ (myPlantData.timeBeforeMature * currentCycle + myPlantData.timeBeforeFruit * (currentCycle - 1) + myPlantData.timeToGatherFruit * (currentCycle - 1)) + ", next fruit time"+ (myPlantData.timeBeforeMature * currentCycle + myPlantData.timeBeforeFruit * currentCycle + myPlantData.timeToGatherFruit * (currentCycle - 1)));
            }

            //growing part of the cycle
            if (mySoils.mySoilsData.plant_time > (myPlantData.timeBeforeMature * currentCycle + myPlantData.timeBeforeFruit * (currentCycle - 1) + myPlantData.timeToGatherFruit * (currentCycle - 1)) && mySoils.mySoilsData.plant_time < (myPlantData.timeBeforeMature * currentCycle + myPlantData.timeBeforeFruit * currentCycle + myPlantData.timeToGatherFruit * (currentCycle - 1)) && mySoils.mySoilsData.plant_time < myPlantData.lifetime && !myPlantData.isMature)
            {
                //Debug.Log("it is time for the plant to mature!!! Cycle:"+ currentCycle);
                if (myPlantData.plantHappiness >= 1)
                {
                    myPlantData.DisplayMature(currentCycle);
                    UpdatePlantAppearanceHappiness();
                    myPlantData.isMature = true;
                    myPlantData.isFruit = false;
                }

                //destroy fruits from previous cycles, not needed for first cycle as there is no fruits yet
                if (currentCycle > 1)
                {
                    foreach (Transform child in this.transform)
                    {
                        if (child.tag == "fruit")
                        {
                            GameObject.Destroy(child.gameObject);
                        }
                    }
                }

                //remove water and nutrients from soil according to plant happiness
                switch (myPlantData.plantHappiness)
                {
                    case 2:
                        mySoils.mySoilsData.water_lvl -= myPlantData.happyWaterConsumption;
                        mySoils.mySoilsData.nutrient_lvl -= myPlantData.happyNutrientConsumption;

                        //we can't get negative water or nutrient
                        if (mySoils.mySoilsData.water_lvl < 0)
                            mySoils.mySoilsData.water_lvl = 0;
                        if (mySoils.mySoilsData.nutrient_lvl < 0)
                            mySoils.mySoilsData.nutrient_lvl = 0;
                        break;
                    case 1:
                        mySoils.mySoilsData.water_lvl -= myPlantData.okWaterConsumption;
                        mySoils.mySoilsData.nutrient_lvl -= myPlantData.okNutrientConsumption;

                        //we can't get negative water or nutrient
                        if (mySoils.mySoilsData.water_lvl < 0)
                            mySoils.mySoilsData.water_lvl = 0;
                        if (mySoils.mySoilsData.nutrient_lvl < 0)
                            mySoils.mySoilsData.nutrient_lvl = 0;
                        break;

                    /*  //lazy way of preventing bad state plants to drain the soil: comment the issue ahem
                    case 0:
                        mySoils.mySoilsData.water_lvl -= myPlantData.badWaterConsumption;
                        mySoils.mySoilsData.nutrient_lvl -= myPlantData.badNutrientConsumption;
                        break;*/

                    default:
                        //print("No happiness level detected");
                        break;
                }

                //#todo: prevent from removing water and nutrient if it makes it goes to 0
                //#todo: prevent from running several time if it is not mature because bad state (now it works but badly done)
            }

            //fruit part of the cycle
            if ((mySoils.mySoilsData.plant_time > ((myPlantData.timeBeforeMature + myPlantData.timeBeforeFruit) * currentCycle) + myPlantData.timeBeforeFruit * (currentCycle - 1)) && (mySoils.mySoilsData.plant_time < ((myPlantData.timeBeforeMature * (currentCycle + 1) + myPlantData.timeBeforeFruit + myPlantData.timeToGatherFruit) * currentCycle)) && (mySoils.mySoilsData.plant_time < myPlantData.lifetime) && !myPlantData.isFruit)
            {
                //Debug.Log("it is time for the plant to have fruits!!!");
                if (myPlantData.plantHappiness >= 1)
                {
                    UpdatePlantAppearanceHappiness();
                    myPlantData.isFruit = true;
                    myPlantData.isMature = false;
                    myPlantData.DisplayReproduction(myPlantData.plantHappiness, mySoils, currentCycle);//function that will manage the plant reproduction with fruits, suckers and all
                }

                //remove water and nutrients from soil according to plant happiness
                switch (myPlantData.plantHappiness)
                {
                    case 2:
                        if (mySoils.mySoilsData.water_lvl > 0)
                            mySoils.mySoilsData.water_lvl -= myPlantData.happyWaterConsumption;
                        if (mySoils.mySoilsData.nutrient_lvl > 0)
                            mySoils.mySoilsData.nutrient_lvl -= myPlantData.happyNutrientConsumption;
                        break;
                    case 1:
                        if (mySoils.mySoilsData.water_lvl > 0)
                            mySoils.mySoilsData.water_lvl -= myPlantData.okWaterConsumption;
                        if (mySoils.mySoilsData.nutrient_lvl > 0)
                            mySoils.mySoilsData.nutrient_lvl -= myPlantData.okNutrientConsumption;
                        break;

                        //#to change
                        //lazy way of solving the issue that if the plant is in bad state, this is asked every frame and drains the soil up to 0
                    /*
                    case 0:
                        if (mySoils.mySoilsData.water_lvl > 0)
                            mySoils.mySoilsData.water_lvl -= myPlantData.badWaterConsumption;
                        if (mySoils.mySoilsData.nutrient_lvl > 0)
                            mySoils.mySoilsData.nutrient_lvl -= myPlantData.badNutrientConsumption;
                        break;*/
                    default:
                        print("No happiness level detected");
                        break;
                }
            }

            if (mySoils.mySoilsData.plant_time > myPlantData.lifetime)
            {
                //Debug.Log("Plant lifetime is over");

                if (mySoils)
                {
                    mySoils.mySoilsData.is_planted = false;
                    //update the soils status and tells its neighbour there is no more plant there
                    mySoils.UpdateAllSurroundingCount();
                }
                //destroy objet and children
                Destroy(this.gameObject);
            }

            SmallContinuousScale(myPlantData.gameObject, initialPlantSize);
        }
    }

    //update the state of happiness of the plant, its sucker and stalk appearance according to its water, nutrient and neighbour level
    //plantHappiness: 2 = happy, 1 = medium, 0 = bad
    void UpdatePlantAppearanceHappiness()
    {
        //happy state of the plant
        if (mySoils.mySoilsData.water_lvl >= myPlantData.miniHappyWater && mySoils.mySoilsData.water_lvl <= myPlantData.maxHappyWater && mySoils.mySoilsData.nutrient_lvl >= myPlantData.miniHappyNutrient && mySoils.mySoilsData.nutrient_lvl <= myPlantData.maxHappyNutrient && mySoils.plantedNeighbours >= myPlantData.miniHappyNeigbours && mySoils.plantedNeighbours <= myPlantData.maxHappyNeigbours)
        {
            myPlantData.plantHappiness = 2;
            if (myPlantData.healthyPlantMaterial)
            {
                this.GetComponent<Renderer>().material = myPlantData.healthyPlantMaterial;
                if(myPlantData.myMatureForm)
                    myPlantData.myMatureForm.GetComponent<Renderer>().material = myPlantData.healthyPlantMaterial;
            }
                
        }
        //bad state
        else if (mySoils.mySoilsData.water_lvl < myPlantData.miniHappyWater-10 || mySoils.mySoilsData.water_lvl > myPlantData.maxHappyWater+10 || mySoils.mySoilsData.nutrient_lvl < myPlantData.miniHappyNutrient-10 || mySoils.mySoilsData.nutrient_lvl > myPlantData.maxHappyNutrient+10)
        {
            myPlantData.plantHappiness = 0;
            if (myPlantData.badPlantMaterial)
            {
                this.GetComponent<Renderer>().material = myPlantData.badPlantMaterial;
                if (myPlantData.myMatureForm)
                    myPlantData.myMatureForm.GetComponent<Renderer>().material = myPlantData.badPlantMaterial;
            }
        }
        //medium state
        else
        {
            myPlantData.plantHappiness = 1;
            if (myPlantData.mediumPlantMaterial)
            {
                this.GetComponent<Renderer>().material = myPlantData.mediumPlantMaterial;
                if (myPlantData.myMatureForm)
                    myPlantData.myMatureForm.GetComponent<Renderer>().material = myPlantData.mediumPlantMaterial;
            }

        }
    }

    void SmallContinuousScale(GameObject myObject, float myInitialPlantSize)
    {
        float currentSin = (0.95f + (Mathf.Sin(Time.time * 3 + initialTimer)) * 0.05f)* myInitialPlantSize;
        myObject.transform.localScale = new Vector3(currentSin, currentSin, currentSin);
    }

}
