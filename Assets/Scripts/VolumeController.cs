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
       UpdateVolume(); 
    }

    void UpdateVolume()
    { 
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
