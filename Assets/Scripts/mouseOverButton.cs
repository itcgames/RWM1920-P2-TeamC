using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseOverButton : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
, IPointerClickHandler
{
    public AudioClip m_hoverSFX;
    public AudioClip m_clickSFX;
    private AudioSource m_source;
    void Start()
    {
        m_source = transform.parent.GetComponent<AudioSource>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area
        transform.localScale = new Vector2(1.2f, 1.2f);
        m_source.PlayOneShot(m_hoverSFX);
    }

    public void OnPointerExit(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area
        transform.localScale = new Vector2(1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area
        transform.localScale = new Vector2(1f, 1f);
        m_source.PlayOneShot(m_clickSFX);
    }
}
