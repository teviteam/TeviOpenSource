using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twinkleController : MonoBehaviour {

    private Soils soil;
    public GameObject twinklePrefab;
    public List<TwinkleBehaviour> twinkleObjects = new List<TwinkleBehaviour>();

    public float timer = 10;
    public float minFrequency = 0.5f;
    public float maxFrequency = 2;
    public bool notPlayedYet = true;

    // Use this for initialization
    void Start () {

        soil = GetComponent<Soils>();

        if (twinklePrefab)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject go = GameObject.Instantiate(twinklePrefab, transform);
                twinkleObjects.Add(go.GetComponent<TwinkleBehaviour>());
            }
        }

    }

    public void ResetTimer()
    {
        //Debug.Log("reseting");
        timer = Random.Range(minFrequency, maxFrequency);
        notPlayedYet = true;
    }
	
	// Update is called once per frame
	void Update () {

        timer -= 1 * Time.deltaTime;

        if (timer <= 0 && notPlayedYet)
        {
            for (int i = 0; i < twinkleObjects.Count; i++)
            {
                if (twinkleObjects[i].gameObject.activeSelf)
                {
                    twinkleObjects[i].anim.Play("TwinkleAnimation", -1, 0);
                }
                
            }

            notPlayedYet = false;

            ResetTimer();

        }






        if (soil && soil.mySoilsData.nutrient_lvl >= 40)
        {
            if (twinkleObjects.Count >= 1)
            {
                twinkleObjects[0].gameObject.SetActive(true);
                twinkleObjects[1].gameObject.SetActive(true);
            }
        }
        if (soil && soil.mySoilsData.nutrient_lvl >= 20 && soil.mySoilsData.nutrient_lvl < 40)
        {
            if (twinkleObjects.Count >= 1)
            {
                twinkleObjects[0].gameObject.SetActive(true);
                twinkleObjects[1].gameObject.SetActive(false);
            }
        }
        if (soil && soil.mySoilsData.nutrient_lvl < 20)
        {
            if (twinkleObjects.Count >= 1)
            {
                twinkleObjects[0].gameObject.SetActive(false);
                twinkleObjects[1].gameObject.SetActive(false);
            }
        }



    }



}
