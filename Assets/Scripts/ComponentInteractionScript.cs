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
    cakeslice.Outline m_outlineController;

    private bool m_selected;
    [System.NonSerialized]
    public bool m_rightClicked;

    private GameObject m_anchor;
    private GameController m_controller;

    // Start is called before the first frame update
    void Start()
    {
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        m_outlineController = gameObject.GetComponentInChildren<cakeslice.Outline>();
        m_rightClicked = false;
        m_selected = false;
        m_rb2 = gameObject.GetComponent<Rigidbody2D>();
        m_click = false;
        m_dragged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_selected)
        {
            m_selected = !m_controller.IsSimRunning();
        }

        if (m_selected)
        {
            // long click effect
            if (m_dragged || m_click && (Time.time - m_mouseDownTime) >= LONG_CLICK_TIME)
            {
                MoveComponent();
            }

            if (Input.GetMouseButtonDown(1))
            {
                HandleComponentAlteration();
            }
        }

        if (Input.GetMouseButtonDown(0) && !m_click && m_selected)
        {
            DeselectComponent();
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
        if (!m_selected)
        {
            SelectComponent();
        }
    }

    private void OnMouseOver()
    {
        if (!m_selected && Input.GetMouseButtonDown(1))
        {
            m_rightClicked = true;
        }
    }
    private void OnMouseDown()
    {
        m_mouseDownTime = Time.time;
        SetClickStartPosOnObject();
        m_click = true;
    }

    private void OnMouseUp()
    {
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
            if (!m_selected)
            {
                SelectComponent();
            }
            else if (!m_dragged)
            {
                DeselectComponent();
            }
        }

        //reset bools
        m_click = false;
        m_dragged = false;
    }

    private void SelectComponent()
    {
        m_selected = true;
        m_outlineController.eraseRenderer = false;
    }

    private void DeselectComponent()
    {
        m_selected = false;
        m_outlineController.eraseRenderer = true;
    }

    private void HandleComponentAlteration()
    {
        string tag = "";
        if (gameObject.transform.parent != null)
        {
            tag = gameObject.transform.parent.tag;
        }
        else
        {
            tag = gameObject.tag;
        }

        if (tag == "Balloon")
        {
            var interactiveComps = FindObjectsOfType<ComponentInteractionScript>();
            bool anchorSet = false;
            BalloonController balloon = gameObject.GetComponentInChildren<BalloonController>();

            foreach (var comp in interactiveComps)
            {
                if (comp.m_rightClicked)
                {
                    balloon.SetAnchor(comp.gameObject);
                    comp.m_rightClicked = false;
                    anchorSet = true;
                    break;
                }
            }
            if (!anchorSet)
            {
                if (m_anchor != null)
                {
                    Destroy(m_anchor);
                }
                m_anchor = new GameObject("Anchor");
                Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_anchor.transform.position = new Vector3(tempPos.x, tempPos.y, 0.0f);
                balloon.SetAnchor(m_anchor);
            }
        }
    }

    private void SetClickStartPosOnObject()
    {
        Vector2 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_clickStartPosX = mousePos.x - transform.localPosition.x;
        m_clickStartPosY = mousePos.y - transform.localPosition.y;
    }
}
