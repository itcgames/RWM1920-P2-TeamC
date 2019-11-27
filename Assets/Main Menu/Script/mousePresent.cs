using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class mousePresent : MonoBehaviour
{
    private Vector3 m_startPos;
    void Start()
    {

        m_startPos = transform.position;
    }

    void Update(){
        transform.position = m_startPos;

    }

	public void OnPointerEnter(PointerEventData eventData){//When the Mouse is over the UI Element area


    }

    public void OnPointerExit(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area

    }
}
