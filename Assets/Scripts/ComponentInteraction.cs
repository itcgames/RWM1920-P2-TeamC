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
    private Vector2 m_mouseDragStart;



    // Start is called before the first frame update
    void Start()
    {
        m_mouseDragStart = new Vector3(9999, 9999);
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
            // long click effect (consider 
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
                else if(CompareTag("WreckingBall"))
                {
                    RotateAllTowardsMouse();
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !m_click && m_selected)
        {
            UnselectComponent();
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
        if ((Time.time - m_mouseDownTime) < LONG_CLICK_TIME)
        {
            if (m_mouseDragStart.magnitude > 10000)
            {
                m_mouseDragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if ((m_mouseDragStart - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)).magnitude > 0f)
            {
                Debug.Log("mouse dragged");
                //when object is being dragged by mouse, immiadiately consider it as a long click
                m_dragged = true;
                if (!m_selected)
                {
                    SelectComponent();
                }
            } 
        }
        else if (!m_selected)
        {
            Debug.Log("mouse long click");
            SelectComponent();
        }
    }

    private void OnMouseOver()
    {
        //if we hover over the object and RMB click
        if (!m_selected && Input.GetMouseButtonDown(1))
        {
            //flip bool
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
        Debug.Log("mouse down");
        //set time when we clicked to differiciate between short click and long click
        m_mouseDownTime = Time.time;
        SetClickStartPosOnObject();
        m_click = true;
    }

    private void OnMouseUp()
    {
        //if this object has a 2d rigidbody
        if (m_rb2 != null)
        {
            //freeze it
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
                //if we released mouse while not dragging
                UnselectComponent();
            }
        }

        //reset bools
        m_click = false;
        m_dragged = false;
        m_mouseDragStart = new Vector2(9999, 9999);
    }

    private void SelectComponent()
    {
        //select this object
        m_selected = true;
        //reset rightclick just in case
        m_rightClicked = false;

        //for each outline script on this gameobject and its children
        foreach (var outline in m_outlineController)
        {
            //enable outline drawing
            outline.eraseRenderer = false;
        }
    }

    private void UnselectComponent()
    {
        //if we deselected while changing angle...
        if ((CompareTag("Fan") || CompareTag("Cannon")) && m_rightClicked)
        {
            //change angle back to what it was before RMB click
            transform.Find("Pivot").transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_originalAngle);
        }

        //reset bools since we're unselecting
        m_selected = false;
        m_rightClicked = false;

        //for each outline script on this gameobject and its children
        foreach (var outline in m_outlineController)
        {
            //disable outline drawing
            outline.eraseRenderer = true;
        }
    }

    private void HandleComponentAlteration()
    {
        //get tag
        string tag;
        //if current gameObject has a parent, get its tag
        if (gameObject.transform.parent != null)
        {
            tag = gameObject.transform.parent.tag;
        }
        else
        {
            tag = gameObject.tag;
        }

        //depending on tag, handle this object differently
        if (tag == "Balloon")
        {
            HandleBalloon();
        }
        else if (tag == "Fan" || tag == "Cannon")
        {
            //reuse this bool for rotating fan and cannon
            m_rightClicked = !m_rightClicked;
            //save the angle before we start following mouse
            m_originalAngle = gameObject.transform.Find("Pivot").transform.eulerAngles.z;
        }
        else if (tag == "WreckingBall")
        {
            m_rightClicked = !m_rightClicked;
            m_originalAngle = gameObject.transform.Find("Ball").transform.eulerAngles.z;

        }
    }

    private void HandleBalloon()
    {
        //get all interactive scripts
        var interactiveComps = FindObjectsOfType<ComponentInteraction>();
        bool anchorSet = false;
        //get balloon's controller
        BalloonController balloon = gameObject.GetComponentInChildren<BalloonController>();

        //find if any object with interactive script was clicked
        foreach (var comp in interactiveComps)
        {
            //if it has...
            if (comp.m_rightClicked)
            {
                //set anchor to that GameObject
                balloon.SetAnchor(comp.gameObject);
                //reset that object's bool
                comp.m_rightClicked = false;
                //anchor has been set
                anchorSet = true;
                //we're done, break out of loop
                break;
            }
        }

        //if we still haven't anchored
        if (!anchorSet)
        {
            //if we have created an anchor before
            if (m_anchor != null)
            {
                //destory it
                Destroy(m_anchor);
            }

            //create new empty anchor
            m_anchor = new GameObject("Anchor");
            //get mouse pos
            Vector2 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //set anchor to static point where mouse right clicked
            m_anchor.transform.position = new Vector3(tempPos.x, tempPos.y, 0.0f);
            //set the anchor
            balloon.SetAnchor(m_anchor);
        }
    }

    //make GameObject point towards the mouse
    private void RotateTowardsMouse()
    {
        Transform rotatingPart = transform.Find("Pivot");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - rotatingPart.position.y, mousePos.x - rotatingPart.position.x) * Mathf.Rad2Deg;
        rotatingPart.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void RotateAllTowardsMouse()
    {
        Transform anchor = transform.Find("AnchorPoint");
        Transform ball = transform.Find("Ball");
        List<Transform> hinges = new List<Transform>();
        for(int index = 0; index < 4; index++)
        {
            hinges.Add(transform.Find("Hinge" + (index + 1).ToString()));
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - anchor.position.y, mousePos.x - anchor.position.x) * Mathf.Rad2Deg;
        anchor.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
        float dist = 0.3f;
        for (int index = 0; index < 4; index++)
        {
            hinges[index].rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
            hinges[index].position = new Vector2(dist*(Mathf.Cos(angle * Mathf.Deg2Rad)), dist*(Mathf.Sin(angle * Mathf.Deg2Rad)));
            dist += 0.5f;
        }
        dist += 0.3f;
        ball.position = new Vector2(dist * (Mathf.Cos(angle * Mathf.Deg2Rad)), dist * (Mathf.Sin(angle * Mathf.Deg2Rad)));

        List<Transform> children = new List<Transform>();
        for(int index = 0; index < transform.childCount; index++)
        {
            children.Add(transform.GetChild(index));
        }

        transform.DetachChildren();
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + 180.0f));
        foreach  (Transform child in children)
        {
            child.parent = transform;
        }
    }

    /// <summary>
    /// Gets the position of the object so it doesnt snap its center to the mouse
    /// </summary>
    private void SetClickStartPosOnObject()
    {
        Vector2 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_clickStartPosX = mousePos.x - transform.localPosition.x;
        m_clickStartPosY = mousePos.y - transform.localPosition.y;
    }
}
