using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScrolling : MonoBehaviour, IDragHandler {

    private bool beingClicked = false; //is the menu being clicked on?
    private float mouseClickX = 0; //xPos of the mouse click
    private float mousePosX = 0;
    private float menuPos = 0;
    private float difference = 0;
    private float time = 0;
    private float speed = 0;
    RectTransform rt;

	// Use this for initialization
	void Start () {

        if (GetComponent<RectTransform>() != null)
        {
            rt = GetComponent<RectTransform>();
        }   
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (beingClicked == false && Input.GetMouseButton(0))
        {
            beingClicked = true;
            mouseClickX = Input.mousePosition.x;
            menuPos = transform.position.x;
        }

        if (beingClicked)
        {
            mousePosX = Input.mousePosition.x;

            difference = mouseClickX - Input.mousePosition.x;
            transform.position = new Vector2(menuPos - difference, transform.position.y);
        }

        
    }

    // Update is called once per frame
    void Update () {

        if (rt.anchoredPosition.x > 0)
        {
            speed = 0;
            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        }
        if (rt.anchoredPosition.x < (0 - rt.rect.width) + 200)
        {
            speed = 0;
            rt.anchoredPosition = new Vector2((0 - rt.rect.width) + 200, rt.anchoredPosition.y);
        }

        if (beingClicked)
        {
            time += 1 * Time.deltaTime;
            speed = (difference / time) / 100;
        }
        else
        {
            time = 0;
            transform.position = new Vector2(transform.position.x - speed, transform.position.y);

            if (speed > -0.1 && speed <= 0.1)
            {
                speed = 0;
            }
            else if(speed > 0.1)
            {
                speed -= (speed/50) + 0.2f;
            }
            else if(speed < -0.1)
            {
                speed += (speed/50) + 0.2f;
            }
            
        }

        if (beingClicked == true && !Input.GetMouseButton(0))
        {
         
            beingClicked = false;
            
        }

    }
}
