using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject m_obj;
    private GameController m_controller;
    private bool m_followPointer;
    private GameObject m_placeObject = null;

    void Start()
    {
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!m_controller.IsSimRunning())
        {
            Vector3 spawnPos = transform.position;
            spawnPos.z = 0;
            m_placeObject = Instantiate(m_obj, spawnPos, Quaternion.identity);
            m_followPointer = true;

            ComponentInteraction placedObjectScript = m_placeObject.GetComponent<ComponentInteraction>();
            placedObjectScript.Init();
            
            var components =FindObjectsOfType<ComponentInteraction>();
            foreach (var component in components)
            {
                if(component.GetSelected())
                {
                    component.SetSelected(false);
                }
            }
            placedObjectScript.SelectFromGameController();
        }
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        ComponentInteraction placedObjectScript = m_placeObject.GetComponent<ComponentInteraction>();
        placedObjectScript.UnselectFromGameController();

        m_followPointer = false;
    }

    void Update()
    {
        if (m_followPointer && m_placeObject != null)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            m_placeObject.transform.position = newPosition;            
        }
    }
}
