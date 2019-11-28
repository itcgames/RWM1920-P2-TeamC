﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject m_obj;
    public GameObject m_controller;
    private bool m_followPointer;
    private GameObject m_placeObject = null;

    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!m_controller.GetComponent<GameController>().IsSimRunning())
        {
            m_placeObject = Instantiate(m_obj, this.transform.position, Quaternion.identity);
            m_followPointer = true;
        }
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        m_followPointer = false;
    }

    void Update()
    {
        if(m_followPointer && m_placeObject != null)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 1;
            m_placeObject.transform.position = newPosition;
        }
    }
}
