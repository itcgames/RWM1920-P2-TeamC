using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    void Start()
    {
        AudioListener.volume = OptionsData.Volume;
    }

    void Update()
    {
       if(OptionsData.VolumeChanged)
        {
            OptionsData.VolumeChanged = false;
            UpdateVolume();
        }
    }

    void UpdateVolume()
    {
        Debug.Log("HERE ONCE");
        if (OptionsData.Mute)
        {
            AudioListener.volume = 0.0f;
        }
        else
        {
            AudioListener.volume = OptionsData.Volume;
        }
    }
}
