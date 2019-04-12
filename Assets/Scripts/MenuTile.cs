using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTile : MonoBehaviour {

    public InventoryEntry inventoryEntry;
    public Image menuTileBackground; //the image that displays the circle backgrund for the menu tile
    public Sprite menuTileSelected; 
    public Sprite menuTileNotSelected;


    private GameObject inventoryObj;
    private Inventory inventory;
    //public  Button button;
    private GameObject myScriptsObject;
    private UIManager uiManager;

    public float selectedSize = 1.15f;
    private void Awake()
    {
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }

    void Start()
    {


        menuTileBackground = GetComponent<Image>();

        inventoryObj = GameObject.Find("InventoryObject");
        if(inventoryObj)
        {
            inventory = inventoryObj.GetComponent<Inventory>();
        }

        //button = GetComponent<Button>();

        myScriptsObject = GameObject.Find("MyScripts");
        if (myScriptsObject)
        {
            uiManager = myScriptsObject.GetComponent<UIManager>();
        }

        UnSelect();
        
  
    }


    public void UnSelect()
    {
        if (menuTileBackground != null && menuTileNotSelected != null)
        {
            menuTileBackground.sprite = menuTileNotSelected;
        }
        transform.localScale = new Vector2(1f, 1f);
    }

    public void Select()
    {
        if (menuTileBackground != null && menuTileSelected != null)
        {
            menuTileBackground.sprite = menuTileSelected;
        }
        transform.localScale = new Vector2(1.07f, 1.07f);
    }

    public void SetCurrentItem()
    {
        if (inventoryEntry != null && inventory.currentItem != inventoryEntry)
        {
            inventory.currentItem = inventoryEntry;
            uiManager.ColorSelectedMenuTile(this);


        }
        else
        {
            inventory.currentItem = null;
            uiManager.ColorSelectedMenuTile(this);

        }

    }    

}
