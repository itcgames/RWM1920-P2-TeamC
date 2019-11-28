using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsData : MonoBehaviour
{
    private float m_volumeSetting = 1;
    private bool m_mute = false;

    public float GetVolume()
    {
        return m_volumeSetting;
    }

    public bool GetMute()
    {
        return m_mute;
    }

    public void SetMute(bool t_mute)
    {
        m_mute = t_mute;
        Debug.Log("HERE");
    }

    public void SetVolume(float t_volume)
    {
        m_volumeSetting = t_volume;
    }
}
