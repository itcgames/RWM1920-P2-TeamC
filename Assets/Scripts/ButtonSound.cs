using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
, IPointerEnterHandler

{
    public AudioClip m_hoverClip;
    public AudioClip m_clickClip;
    private AudioSource m_source;

    void Start()
    {
        m_source = gameObject.transform.GetComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area

        m_source.PlayOneShot(m_hoverClip);
    }
    public void buttonClicked()
    {
        m_source.PlayOneShot(m_clickClip);
    }
}
