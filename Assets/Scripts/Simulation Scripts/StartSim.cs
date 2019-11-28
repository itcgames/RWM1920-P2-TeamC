using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartSim : MonoBehaviour, IPointerClickHandler
{
    public GameObject m_controller;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GameController script = m_controller.GetComponent<GameController>();
        script.StartSim();
    }
}
