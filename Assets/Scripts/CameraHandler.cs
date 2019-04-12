using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//based on https://kylewbanks.com/blog/unity3d-panning-and-pinch-to-zoom-camera-with-touch-and-mouse-input

public class CameraHandler : MonoBehaviour
{

    public float PanSpeed = 10.0f;
    private static readonly float ZoomSpeedTouch = 0.01f;
    private static readonly float ZoomSpeedMouse = 1.0f;

    //private static readonly float[] BoundsX = new float[] { -2f, 6.5f };
    //private static readonly float[] BoundsZ = new float[] { -4f, -10f };
    private static readonly float[] ZoomBounds = new float[] { 20.0f, 120.0f };
    private static readonly float[] CamMovZoomBounds = new float[] { -10.0f, -0.1f };

    public Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    public float xLimitLow = -6.0f;
    public float xLimitHigh = 10.0f;
    public float yLimitLow = 3.0f;
    public float yLimitHigh = 20.0f;
    public float zLimitLow = -8.0f;
    public float zLimitHigh = 4.0f;

    void Update()
    {


        if (cam.transform.position.x < xLimitLow)
        {
            cam.transform.position = new Vector3 (xLimitLow, cam.transform.position.y, cam.transform.position.z);
            Debug.Log("ASS");
        }
        if (cam.transform.position.x > xLimitHigh)
        {
            cam.transform.position = new Vector3 (xLimitHigh, cam.transform.position.y, cam.transform.position.z);
        }
        if (cam.transform.position.y < yLimitLow)
        {
            cam.transform.position = new Vector3 (cam.transform.position.x, yLimitLow, cam.transform.position.z);
        }
        if (cam.transform.position.y > yLimitHigh)
        {
            cam.transform.position = new Vector3 (cam.transform.position.x, yLimitHigh, cam.transform.position.z);
        }
        if (cam.transform.position.z < zLimitLow)
        {
            cam.transform.position = new Vector3 (cam.transform.position.x, cam.transform.position.y, zLimitLow);
        }
        if (cam.transform.position.z > zLimitHigh)
        {
            cam.transform.position = new Vector3 (cam.transform.position.x, cam.transform.position.y, zLimitHigh);
        }

        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleTouch()
    {
        Debug.Log("touch");
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
       
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            lastPanPosition = Input.mousePosition;
            //Debug.Log("lastPanPosition" + lastPanPosition);
        }
        else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PanCamera(Input.mousePosition);
            //Debug.Log("PanCamera Input.mousePosition " + Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        //Debug.Log("pan");
                // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
        //move.y = cam.transform.position.y;

        //Debug.Log("move" + move);
        
        // Perform the movement
        cam.transform.Translate(move, Space.World);
                
        /*
        // Ensure the camera remains within bounds.
        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        cam.transform.position = pos;          
        */

        // Cache the position
        lastPanPosition = newPanPosition;

        /*
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
        */
    }

    void ZoomCamera(float offset, float speed)
    {
        //Debug.Log("zoom");
        if (offset == 0)
        {
            return;
        }
        
        cam.transform.Translate(new Vector3(0, 0, (offset * speed)), Space.Self);
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
        //PanSpeed = Mathf.Clamp(cam.fieldOfView/5, 4.0f, 20.0f);
    }
}
