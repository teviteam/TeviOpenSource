using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManagement : MonoBehaviour {

    public AudioSource backgroundMusicGarden;
    public AudioSource backgroundMusicOutside;

    public Scrollbar scrollbarMusicControl;
    public Scrollbar scrollbarSoundControl;

    private float musicVolume=1;
    private float soundVolume=1;

    //change of the music sound according to the UI menu, called by the scrollbar
    public void ChangeMusicVolume() {
        if (scrollbarMusicControl)
        {
            musicVolume = scrollbarMusicControl.value;
            if (backgroundMusicGarden)
            {
                backgroundMusicGarden.volume = musicVolume;
            }
            if (backgroundMusicOutside)
            {
                backgroundMusicGarden.volume = musicVolume;
            }
        }
    }

    //#to do when the sound will be added, called by the scrollbar
    public void ChangeSoundVolume()
    {
    }

}
