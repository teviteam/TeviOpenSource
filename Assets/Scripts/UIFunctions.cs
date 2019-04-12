using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour {

    public Canvas myCanvas;
    public float myMargins = 300;

    public GameObject[] menusToSwitchOff;
    public GameObject questTextObj;

    public void DeactivateThis(GameObject myObject)
    {
        myObject.SetActive(false);
    }

    //when called deactivate the GO if active, or activate if inactive
    public void ButtonSwitch(GameObject myObject)
    {
        if (menusToSwitchOff.Length > 0) {
        foreach (var item in menusToSwitchOff)
        {
            item.SetActive(false);
        }
    }

        if (myObject)
        {
            if (myObject.activeSelf == false)
            {
                myObject.SetActive(true);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                myObject.SetActive(false);
            }
        }
    }

    public void SwitchOffAllMenus()
    {
        if (menusToSwitchOff.Length > 0) {
        foreach (var item in menusToSwitchOff)
        {                  
            if (item)
            {
                item.SetActive(false);
            }
        }
        /*if (questTextObj) { 
            questTextObj.GetComponent<QuestManager>().DeactivateBox();
            }*/
        }
    }

    //exiting the app if not in editor mode
    public void ExitApp()
    {
        /*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
        }*/
        GameObject ConnectionInfosObject = GameObject.Find("ConnectionInfos");
        Destroy(ConnectionInfosObject);
        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
    }

    //adapt UI to screen width, as height is fixed (-margins*2)
    public void AdaptUIWidth(GameObject myUItoResize)
    {
        if(myCanvas)
        {
            float screenWidth = myCanvas.GetComponent<RectTransform>().rect.width;
            //Debug.Log("screenWidth : " + screenWidth);

            RectTransform myRect = myUItoResize.GetComponent<RectTransform>();
            Vector2 currentSizeDelta = myRect.sizeDelta;

            float tempsizex = screenWidth - myMargins * 2;
            myRect.sizeDelta = new Vector2(tempsizex, currentSizeDelta.y);
        }
    }

    public void DeleteAccount()
    {
        GameObject connexionObj = GameObject.Find("ConnectionInfos");
        if (connexionObj)
        {
            connexionObj.GetComponent<Connecting>().StartDeletingAccount();
        }
    }

    public void SetActiveBasedOnBool (GameObject objToSet, bool boolean)
    {
        objToSet.SetActive(boolean);
    }
}
