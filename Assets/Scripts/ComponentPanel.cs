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
            m_placeObject = Instantiate(m_obj, this.transform.position, Quaternion.identity);
            m_followPointer = true;

            //m_placeObject.GetComponent<Collider2D>().enabled = false;
            //foreach (var c in m_placeObject.GetComponentsInChildren<Collider2D>())
            //{
            //    c.enabled = false;
            //}

            ComponentInteraction placedObjectScript = m_placeObject.GetComponent<ComponentInteraction>();
            placedObjectScript.Init();
            placedObjectScript.SelectFromGameController();
        }
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //m_placeObject.GetComponent<Collider2D>().enabled = true;
        //foreach (var c in m_placeObject.GetComponentsInChildren<Collider2D>())
        //{
        //    c.enabled = true;
        //}

        ComponentInteraction placedObjectScript = m_placeObject.GetComponent<ComponentInteraction>();
        placedObjectScript.UnselectFromGameController();

        m_followPointer = false;
    }

    void Update()
    {
        if (m_followPointer && m_placeObject != null)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 1;
            m_placeObject.transform.position = newPosition;            
        }
    }
}
