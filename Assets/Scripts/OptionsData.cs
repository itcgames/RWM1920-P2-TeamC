using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsData
{
    private static float m_volume = 1;
    private static bool m_mute = false;
    private static bool m_volumeChanged = false;


    public static float Volume
    {
        get
        {
            return m_volume;
        }
        set
        {
            m_volume = value;
        }
    }

    public static bool Mute
    {
        get
        {
            return m_mute;
        }
        set
        {
            m_mute = value;
        }
    }
    public static bool VolumeChanged
    {
        get
        {
            return m_volumeChanged;
        }
        set
        {
            m_volumeChanged = value;
        }
    }



}
