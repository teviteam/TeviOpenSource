using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroAnim : MonoBehaviour {

    public bool skipIntro = false;

    private GameObject connectionObj;
    private bool myNewPlayer;

    private void Start()
    {
        connectionObj = GameObject.Find("ConnectionInfos");
        if (connectionObj)
        {
            myNewPlayer = connectionObj.GetComponent<Connecting>().newPlayer;
        }

        if (skipIntro||!myNewPlayer) {
            disableAll();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            disableAll();
        }
    }

    public void disableAll  () {

        gameObject.SetActive(false);

        gameObject.GetComponent<VideoPlayer>().enabled = false;
    }
}
