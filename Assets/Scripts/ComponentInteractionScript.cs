using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentInteractionScript : MonoBehaviour
{
    private const float LONG_CLICK_TIME = 0.1f;

    private float m_clickStartPosX;
    private float m_clickStartPosY;
    private Rigidbody2D m_rb2;
    private bool m_click;
    private bool m_dragged;
    private float m_mouseDownTime;
    GameObject m_selectionIndicator;

    private bool m_selected;

    // Start is called before the first frame update
    void Start()
    {
        m_selectionIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        m_selectionIndicator.name = "selection_indicator";
        m_selectionIndicator.SetActive(false);


        m_selected = false;
        m_rb2 = gameObject.GetComponent<Rigidbody2D>();
        m_click = false;
        m_dragged = false;
    }

    // Update is called once per frame
    void Update()
    {
        // long click effect
        if (m_dragged || m_click && (Time.time - m_mouseDownTime) >= LONG_CLICK_TIME)
        {
            MoveComponent();
        }
    }

    void MoveComponent()
    {
        if (m_rb2 != null)
        {
            m_rb2.Sleep();
            m_rb2.velocity = Vector2.zero;
            m_rb2.freezeRotation = true;
            m_rb2.angularVelocity = 0.0f;
        }

        Vector2 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        gameObject.transform.localPosition = new Vector2(mousePos.x - m_clickStartPosX, mousePos.y - m_clickStartPosY);
    }

    private void OnMouseDrag()
    {
        //when mouse is being dragged, immiadiately consider it as a long click
        m_dragged = true;
    }

    private void OnMouseUp()
    {
        m_click = false;
        m_dragged = false;

        if (m_rb2 != null)
        {
            m_rb2.freezeRotation = false;
            if (m_rb2.IsSleeping())
            {
                m_rb2.WakeUp();
            }
        }
        //if time LMB was pressed down is less than what we consider a long click do stuff
        if ((Time.time - m_mouseDownTime) < LONG_CLICK_TIME)
        {
            // short click effect here
            if (m_selected)
            {
                //DeselectComponent();
            }
            else
            {
                SelectComponent();
            }
            Debug.Log("Object clicked: " + gameObject.name);

        }
    }

    private void SelectComponent()
    {
        //m_selected = true;

        m_selectionIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        m_selectionIndicator.transform.position = m_selectionIndicator.transform.parent.position;/*.position = new Vector3(0, 0.5f, 0);*/
    }

    private void OnMouseDown()
    {
        m_mouseDownTime = Time.time;
        SetClickStartPosOnObject();
        m_click = true;
    }

    private void SetClickStartPosOnObject()
    {
        Vector2 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_clickStartPosX = mousePos.x - transform.localPosition.x;
        m_clickStartPosY = mousePos.y - transform.localPosition.y;
    }
}
