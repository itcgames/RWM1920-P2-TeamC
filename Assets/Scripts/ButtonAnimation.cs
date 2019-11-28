using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("Sprite For When The Button Is Unselected")]
    public Sprite m_unselectedSprite;
    [Tooltip("Sprite For When The Mouse Is Over The Button")]
    public Sprite m_hoveredSprite;
    [Tooltip("Sprite For When The Button Is Clicked")]
    public Sprite m_clickedSprite;

    private Image m_buttonImage;
    private bool m_mouseOver;
    private bool m_mouseClicked;

    void Start()
    {
        m_buttonImage = GetComponent<Image>();
        Debug.Log("Test");
    }

    private void Update()
    {
        if(!m_mouseOver && !m_mouseClicked)
        {
            m_buttonImage.sprite = m_unselectedSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!m_mouseClicked)
        {
            m_buttonImage.sprite = m_hoveredSprite;
        }
        m_mouseOver = true;
    }
  

    public void OnPointerExit(PointerEventData eventData)
    {
        m_mouseOver = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_buttonImage.sprite = m_clickedSprite;
        m_mouseClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(m_mouseOver)
        {
            m_buttonImage.sprite = m_hoveredSprite;
        }
        m_mouseClicked = false;
    }
}
