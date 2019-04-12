using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseOverDoNotPlant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private UIManager uiManager;


    void Start()
    {

        if (GameObject.Find("MyScripts"))
        {
            uiManager = GameObject.Find("MyScripts").GetComponent<UIManager>();
        }
        else
        {
            Debug.Log("Error: script OnMouseOverDoNotPlant cannot find Object 'MyScripts");
        }

    }





    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiManager != null)
        {
            uiManager.doNotPlant = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiManager != null)
        {
            uiManager.doNotPlant = false;
        }
    }

    public void OnDisable()
    {
        if (uiManager != null)
        {
            uiManager.doNotPlant = false;
        }
    }


}
