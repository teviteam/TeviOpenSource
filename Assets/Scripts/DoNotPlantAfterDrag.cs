using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotPlantAfterDrag : MonoBehaviour {

    public float plantingDistanceCap;
    private UIManager uiManager;
    private Vector3 mouseStartPos;
    private bool dragging = false;

	// Use this for initialization
	void Start () {

        uiManager = GameObject.Find("MyScripts").GetComponent<UIManager>();

        if (uiManager == null)
        {
            Debug.Log("Error: Script DoNotPlantAfterDrag cannot find GameOBject 'MyScript' or its component Script UIManager");
        }
		
	}
	
	// Update is called once per frame
	void LateUpdate () {


        
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
            dragging = true;
        }

        if (dragging && uiManager != null)
        {
            if (Vector3.Distance(mouseStartPos, Input.mousePosition) > plantingDistanceCap)
            {
                uiManager.doNotPlant = true;

                if (Input.GetMouseButtonUp(0))
                {
                    uiManager.doNotPlant = false;
                    dragging = false;
                    return;
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    dragging = false;
                }
            }

        }


        
		
	}

}
