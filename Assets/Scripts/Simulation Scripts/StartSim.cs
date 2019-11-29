using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartSim : MonoBehaviour, IPointerClickHandler
{
    private GameController m_controller;

    void Start()
    {
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        m_controller.StartSim();
    }
}
