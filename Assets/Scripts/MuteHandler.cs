using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteHandler : MonoBehaviour
{
    public void updateMute(bool t_val)
    {
        OptionsData.Mute = t_val;
        OptionsData.VolumeChanged = true;
    }
}
