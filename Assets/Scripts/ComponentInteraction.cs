using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentInteraction : MonoBehaviour
{
    private const float LONG_CLICK_TIME = 0.1f;

    private float m_clickStartPosX;
    private float m_clickStartPosY;
    private Rigidbody2D m_rb2;
    private bool m_click;
    private bool m_dragged;
    private float m_mouseDownTime;
    private List<cakeslice.Outline> m_outlineController;

    private bool m_selected;
    [System.NonSerialized]
    public bool m_rightClicked;

    private GameObject m_anchor;
    private float m_originalAngle;

    // Start is called before the first frame update
    void Start()
    {
        m_outlineController = new List<cakeslice.Outline>(gameObject.GetComponentsInChildren<cakeslice.Outline>());
        m_rightClicked = false;
        m_selected = false;
        m_rb2 = gameObject.GetComponent<Rigidbody2D>();
        m_click = false;
        m_dragged = false;
    }

    // Update is called once per frame
    void Update()
    {
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
            if (m_rightClicked)
            {
                if (CompareTag("Fan") || CompareTag("Cannon"))
                {
                    RotateTowardsMouse();
                }
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

    // weird bug with fan
    //Replication process:
    //1. Run scene
    //2. disable fan area script
    //3. drag cube/sphere above particles and drop it
    //4. select fan
    //5. right click while fan selected
    //6. left click on fan while selected
    //7. Now cant select object in Fan Area?????
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
        m_rightClicked = false;

        foreach (var outline in m_outlineController)
        {
            outline.eraseRenderer = false;
        }
    }

    private void DeselectComponent()
    {
        if ((CompareTag("Fan") || CompareTag("Cannon")) && m_rightClicked)
        {
            //transform.rotation.SetLookRotation(new Vector3(0, 0, ));
            transform.Find("Pivot").transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_originalAngle);
        }

        m_selected = false;
        m_rightClicked = false;

        foreach (var outline in m_outlineController)
        {
            outline.eraseRenderer = true;
        }
    }

    private void HandleComponentAlteration()
    {
        string tag;
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
            HandleBalloon();
        }
        else if (tag == "Fan" || tag == "Cannon")
        {
            m_rightClicked = !m_rightClicked;

            m_originalAngle = gameObject.transform.Find("Pivot").transform.eulerAngles.z;

            //if (tag == "Fan")
            //{

            //}
        }
    }

    private void HandleBalloon()
    {
        var interactiveComps = FindObjectsOfType<ComponentInteraction>();
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

    private void RotateTowardsMouse()
    {
        Transform rotatingPart = transform.Find("Pivot");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - rotatingPart.position.y, mousePos.x - rotatingPart.position.x) * Mathf.Rad2Deg;
        rotatingPart.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void SetClickStartPosOnObject()
    {
        Vector2 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_clickStartPosX = mousePos.x - transform.localPosition.x;
        m_clickStartPosY = mousePos.y - transform.localPosition.y;
    }
}
