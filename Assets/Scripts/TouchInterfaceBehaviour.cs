using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ButtonClickBehaviour;

public class TouchInterfaceBehaviour : MonoBehaviour
{
    //Need to do:
    /* Check if any buttons pressed
     * buttons behaviour - ie what they do
     * position buttons correctly around object
     */

    private GameObject m_component;

    private List<Transform> buttons;
    bool moveCompoenet;
    bool rightClick;

    float m_clickStartPosX;
    float m_clickStartPosY;


    bool firstTimeMove;

    void Start()
    {
        //rightClick = false;
        //firstTimeMove = false;
        //moveCompoenet = false;
        //m_component = transform.parent.gameObject;
        //buttons = new List<Transform>();
        //foreach(Transform child in transform)
        //{
        //    buttons.Add(child);
        //}

        //float scalar = 0.6f;

        //Vector3 componentPos = m_component.transform.position;

        ////cannon - bigger scalar
        //// fan - move left
        //// wrecking ball - move far left && bigger scalar
        //switch (m_component.tag)
        //{
        //    case "Cannon":
        //        scalar = 0.8f;
        //        break;
        //    case "Fan":
        //        {
        //            Vector3 pos = m_component.transform.Find("fanBase").position;
        //            pos.y += 0.25f;
        //            componentPos = pos;
        //        }
        //        break;
        //    case "Ballon":
        //        break;
        //    case "WreckingBall":
        //        {
        //            Vector3 pos = m_component.transform.Find("Ball").position;
    
        //            componentPos = pos;
        //            scalar = 0.8f;
        //        }
        //        break;
        //    default:
        //        break;
        //}





        //foreach(Transform button in buttons)
        //{
        //    Vector3 buttonPosition = new Vector3(0.0f, 0.0f, 0.0f);
        //    switch (button.name)
        //    {
        //        case "MoveButton":
        //            buttonPosition = new Vector3(1.0f, 1.0f, 0.0f);
        //            break;
        //        case "RotateButton":
        //            buttonPosition = new Vector3(1.25f, 0.25f, 0.0f);
        //            break;
        //        case "DeleteButton":
        //            buttonPosition = new Vector3(-1.0f, -1.0f, 0.0f);
        //            break;
        //        case "CancelButton":
        //            buttonPosition = new Vector3(-1.0f, 1.0f, 0.0f);
        //            break;
        //        default:
        //            break;
        //    }
        //    buttonPosition *= scalar;
        //    button.localPosition = buttonPosition;
        //}
        //transform.position = componentPos;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnChildClick(string childName)
    {
        ComponentInteraction activeObject = null;
        var objects = FindObjectsOfType<ComponentInteraction>();
        foreach (ComponentInteraction obj in objects)
        {
            if (obj.GetSelected())
            {
                activeObject = obj;
            }
        }
        if (activeObject != null)
        {
            activeObject.UpdateButtonPressed(childName);
        }
        //GetComponentInParent<ComponentInteraction>().UpdateButtonPressed(childName);
    }
}





