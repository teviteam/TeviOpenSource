using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script on Myscripts object in main scene
public class LoadOnlineImage : MonoBehaviour {

    public Image ImageToLoadTheTextureOn;
    public Text NoImageYetText;
    public GameObject ArrowRight;
    public GameObject ArrowLeft;
    public GameObject ScientificName;
    public GameObject CommonName;

    private int currentImage = 0;

    //we access inventory to get hold of 
    private GameObject inventoryObj;
    private Inventory inventory;

    GameObject ConnectionInfosObject;

    void Start()
    {
        inventoryObj = GameObject.Find("InventoryObject");
        if (inventoryObj)
        {
            inventory = inventoryObj.GetComponent<Inventory>();
        }
        ConnectionInfosObject = GameObject.Find("ConnectionInfos");
    }

    //Function added to the plantnet gallery UI menu to load plant pictures
    public void DisplayPlantnetPicturesGallery()
    {
        Texture2D myGalleryTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
        if (inventory.plantnetResultDatas.Count > 0)
        {
            //we display the arrows only if there is more than 1 image
            if (inventory.plantnetResultDatas.Count > 1)
            {
                ArrowRight.SetActive(true);
                ArrowLeft.SetActive(true);
            }
            else
            {
                ArrowRight.SetActive(false);
                ArrowLeft.SetActive(false);
            }

            if(ScientificName&& CommonName)
            { 
                //we display the names
                ScientificName.SetActive(true);
                CommonName.SetActive(true);
                ScientificName.GetComponent<Text>().text = inventory.plantnetResultDatas[currentImage].plant0;
                CommonName.GetComponent<Text>().text = inventory.plantnetResultDatas[currentImage].common0;
            }

            NoImageYetText.gameObject.SetActive(false);
            ImageToLoadTheTextureOn.gameObject.SetActive(true);
            string serverAddress = ConnectionInfosObject.GetComponent<Connecting>().website;
            string imageUrl = serverAddress+"kreskanta/images_upload/" + inventory.plantnetResultDatas[currentImage].image_name+".png";
            Debug.Log("url of the image to load" + imageUrl);            
            StartCoroutine(setImage(imageUrl, myGalleryTexture));
        }
        else
        {
            NoImageYetText.gameObject.SetActive(true);
            ImageToLoadTheTextureOn.gameObject.SetActive(false);
            ScientificName.SetActive(false);
            CommonName.SetActive(false);
        }
    }

    IEnumerator setImage(string url, Texture2D myTexture)
    {
        //we verify there is a UI image and a texture to load on
        if (ImageToLoadTheTextureOn)
        {
            Debug.Log("Begin loading the image "+ currentImage);

            WWW www = new WWW(url);
            yield return www;
            www.LoadImageIntoTexture(myTexture);
            www.Dispose();
            www = null;

            //adapt UI image size to the proportions of the initial picture
            print("Size is " + myTexture.width + " by " + myTexture.height);
            float imageWidth = myTexture.width;
            float imageHeight = myTexture.height;
            float myImageRatio = imageWidth / imageHeight;
            Debug.Log("myImageRatio:" + myImageRatio+ " ui size width" +ImageToLoadTheTextureOn.rectTransform.sizeDelta.x);
            ImageToLoadTheTextureOn.rectTransform.sizeDelta = new Vector2(myTexture.width, myTexture.height);

            //scale to fit the screen
            float intendedWidth = 700;
            float scaleFactor = intendedWidth / myTexture.width;
            ImageToLoadTheTextureOn.rectTransform.localScale = new Vector3 (scaleFactor, scaleFactor, 1); 

            //to load a texture on UI we have to create a sprite
            Rect rec = new Rect(0, 0, myTexture.width, myTexture.height);
            Sprite myGallerySprite = Sprite.Create(myTexture, rec, new Vector2(0, 0), 1);

            //apply the sprite created to the UI
            ImageToLoadTheTextureOn.GetComponent<Image>().sprite = myGallerySprite;

            //rotate if not portrait and not turned already
            if ((myTexture.width > myTexture.height) && ImageToLoadTheTextureOn.transform.rotation==Quaternion.identity)
            {
                ImageToLoadTheTextureOn.transform.Rotate(new Vector3(0, 0, 270));
            }
        }
        else
        {
            Debug.Log("Not texture to load on");
        }
    }

    public void NextImage()
    {
        if (currentImage + 1 < inventory.plantnetResultDatas.Count)
        {
            currentImage++;
        }
        else
        {
            currentImage = 0;
        }
        Debug.Log("current image: " + currentImage);
        DisplayPlantnetPicturesGallery();
    }

    public void PreviousImage()
    {
        if (currentImage - 1 >= 0)
        {
            currentImage--;
        }
        else
        {
            currentImage = inventory.plantnetResultDatas.Count-1;
        }
        Debug.Log("current image: " + currentImage);
        DisplayPlantnetPicturesGallery();
    }
    /*
    public int getCameraPhotoOrientation(Context context, Uri imageUri, string imagePath)
    {
        int rotate = 0;
        try
        {
            context.getContentResolver().notifyChange(imageUri, null);
            File imageFile = new File(imagePath);

            ExifInterface exif = new ExifInterface(imageFile.getAbsolutePath());
            int orientation = exif.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);

            switch (orientation)
            {
                case ExifInterface.ORIENTATION_ROTATE_270:
                    rotate = 270;
                    break;
                case ExifInterface.ORIENTATION_ROTATE_180:
                    rotate = 180;
                    break;
                case ExifInterface.ORIENTATION_ROTATE_90:
                    rotate = 90;
                    break;
            }

            Log.i("RotateImage", "Exif orientation: " + orientation);
            Log.i("RotateImage", "Rotate value: " + rotate);
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
        return rotate;
    }*/
}
