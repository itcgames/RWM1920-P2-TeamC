using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHandler : MonoBehaviour
{
    public void updateVolume(float t_val)
    {
        OptionsData.Volume = t_val;
        OptionsData.VolumeChanged = true;
    }
}
