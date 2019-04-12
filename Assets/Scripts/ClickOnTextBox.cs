using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickOnTextBox : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler{

    private QuestManager questmanager;
    private Image image;
    private UIManager uiManager;

    void Start()
    {
        if (GetComponentInChildren<QuestManager>())
        {
            questmanager = GetComponentInChildren<QuestManager>();
        }
        else
        {
            Debug.Log("Error: Component(script) ClickOnTextBox cannot find component(script) Questmanager in children");
        }

        if (GetComponent<Image>())
        {
            image = GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: Component(script) ClickOnTextBox cannot find component(Image) Image in gameObject");
        }

        if (GameObject.Find("MyScripts"))
        {
            uiManager = GameObject.Find("MyScripts").GetComponent<UIManager>();
        }
        else
        {
            Debug.Log("Error: script ClickOnTextBox cannot find Object 'MyScripts");
        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image.IsActive())
        {
            if (uiManager != null)
            {
                Debug.Log("Over" + gameObject.name);
                uiManager.doNotPlant = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image.IsActive())
        {
            if (uiManager != null)
            {
                Debug.Log("Exit" + gameObject.name);
                uiManager.doNotPlant = false;
            }
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (questmanager != null)
        {
            questmanager.DeactivateBox();
        }
        if (uiManager != null)
        {
            uiManager.doNotPlant = false;
        }

    }











}
