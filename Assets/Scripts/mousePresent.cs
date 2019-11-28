using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class mousePresent : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
   private bool m_mouseHere = false;
   public bool GetMousePresent()
    {
        return m_mouseHere;
    }
	public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area
        m_mouseHere = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area
        m_mouseHere = false;
    }
}
