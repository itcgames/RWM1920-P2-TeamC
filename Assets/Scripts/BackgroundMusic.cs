using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip m_backgroundMusicSFX;
    private AudioSource m_source;

    void Start()
    {
        m_source = transform.GetComponent<AudioSource>();
        m_source.clip = m_backgroundMusicSFX;
        m_source.volume = 0.3f;
        m_source.Play();
    }

}
