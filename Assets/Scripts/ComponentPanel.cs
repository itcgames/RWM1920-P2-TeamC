using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject m_obj;
    private bool m_followPointer;
    private GameObject m_placeObject = null;

    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        m_placeObject = Instantiate(m_obj, this.transform.position, Quaternion.identity);
        m_followPointer = true;
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        m_followPointer = false;
        Debug.Log(name + "No longer being clicked");
    }

    void Update()
    {
        if(m_followPointer && m_placeObject != null)
        {
            Debug.Log(Input.mousePosition);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 1;
            m_placeObject.transform.position = newPosition;
        }
    }
}
