using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentInteraction : MonoBehaviour
{
    //consts
    private const int SELECTED_COLOUR = 0;
    private const int OBSTRUCTED_COLOUR = 1;
    private const float LONG_CLICK_TIME = 0.1f;
    private const bool DISABLE_RENDERER = true;
    private const bool ENABLE_RENDERER = false;

    //variables
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

    private Vector3 m_startPosition;
    private GameController m_controller;

    private bool m_gameControllerDrag;
    private bool m_init = true;
    private Collider2D[] m_overlapArray;
    private Vector3 m_spawnPos;
    ContactFilter2D m_overlapFilter;

    // Start is called before the first frame update
    void Start()
    {
        if (m_init)
        {
            Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_selected && m_controller != null)
        {
            m_selected = !m_controller.IsSimRunning();
            if (!m_selected)
            {
                UnselectComponent();
            }
        }
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
                else if (CompareTag("WreckingBall"))
                {
                    RotateAllTowardsMouse();
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !m_click && m_selected)
        {
            UnselectComponent();
        }

        if (!ColliderOverlaps())
        {
            ChangeOutlineColour(SELECTED_COLOUR);
            m_startPosition = transform.position;
        }
        else
        {
            ChangeOutlineColour(OBSTRUCTED_COLOUR);
        }
    }

    void MoveComponent()
    {
        if (m_rb2 != null)
        {
            m_rb2.Sleep();
            if (m_rb2.bodyType != RigidbodyType2D.Static)
            {
                m_rb2.velocity = Vector2.zero;
                m_rb2.freezeRotation = true;
                m_rb2.angularVelocity = 0.0f;
            }
        }

        Vector2 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameObject.transform.position = new Vector3(mousePos.x - m_clickStartPosX, mousePos.y - m_clickStartPosY, 0.0f);
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
                //Debug.Log("mouse dragged");
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
            //Debug.Log("mouse long click");
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

    private void OnMouseDown()
    {
        //Debug.Log("mouse down");
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

        //if (!IsOutlineColour(OBSTRUCTED_COLOUR))
        //{
        //    m_startPosition = gameObject.transform.position;
        //}
    }

    public void SelectFromGameController()
    {
        SelectComponent();

        m_gameControllerDrag = true;
    }

    public void UnselectFromGameController()
    {
        //UnselectComponent();
        m_gameControllerDrag = false;

        if (ColliderOverlaps())
        {
            transform.position = m_startPosition;
        }
        if (transform.position == m_spawnPos)
        {
            Destroy(gameObject);
        }

        //if (ColliderOverlaps())
        //{
        //    foreach (var collider in m_overlapArray)
        //    {
        //        if (collider != null)
        //        {
        //            if (collider.CompareTag("GameController"))
        //            {
        //                Destroy(gameObject);
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}
    }

    private void SelectComponent()
    {
        //select this object
        m_selected = true;
        //reset rightclick just in case
        m_rightClicked = false;

        EnableOutlineRenderer(ENABLE_RENDERER);
    }

    private void UnselectComponent()
    {
        if (IsOutlineColour(OBSTRUCTED_COLOUR))
        {
            transform.position = m_startPosition;
        }
        ChangeOutlineColour(SELECTED_COLOUR);

        //if we deselected while changing angle...
        if ((CompareTag("Fan") || CompareTag("Cannon")) && m_rightClicked)
        {
            //change angle back to what it was before RMB click
            transform.Find("Pivot").transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_originalAngle);
        }

        //reset bools since we're unselecting
        m_selected = false;
        m_rightClicked = false;

        //erase the renderer from outline as we unselected
        EnableOutlineRenderer(DISABLE_RENDERER);
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
        //fan and cannon work very similarly
        else if (tag == "Fan" || tag == "Cannon")
        {
            //reuse this bool for rotating fan and cannon
            m_rightClicked = !m_rightClicked;
            if (m_rightClicked)
            {
                //save the angle before we start following mouse
                m_originalAngle = gameObject.transform.Find("Pivot").transform.eulerAngles.z;
            }
            else
            {
                if (IsOutlineColour(OBSTRUCTED_COLOUR))
                {
                    ChangeOutlineColour(SELECTED_COLOUR);
                    transform.Find("Pivot").transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_originalAngle);
                }
            }
        }
        else if (tag == "WreckingBall")
        {
            m_rightClicked = !m_rightClicked;
            m_originalAngle = gameObject.transform.Find("Ball").transform.eulerAngles.z;
        }
    }

    private void HandleBalloon()
    {
        //if we have created an anchor before
        if (m_anchor != null)
        {
            //destory it
            Destroy(m_anchor);
        }

        //get all interactive scripts
        var interactiveComps = FindObjectsOfType<ComponentInteraction>();

        //get balloon's controller
        NewBalloonController balloon = gameObject.GetComponent<NewBalloonController>();

        //find if any object with interactive script was clicked
        foreach (var comp in interactiveComps)
        {
            //if it has...
            if (comp.m_rightClicked)
            {
                //set anchor to that GameObject
                balloon.SetAnchor(comp.gameObject, Vector3.Distance(comp.transform.position, balloon.transform.GetChild(0).position));
                //reset that object's bool
                comp.m_rightClicked = false;

                //anchor set to rigidBody, leave the function
                return;
            }
        }

        //get mouse pos
        Vector2 tempMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<Collider2D>().OverlapPoint(tempMousePos))//If the collider overlaps the mouse coordinates
        {
            //set anchor to the player
            balloon.SetAnchor(player, Vector3.Distance(player.transform.position, balloon.transform.GetChild(0).position));
            //anchor set to rigidBody, leave the function
            return;
        }

        //create new empty anchor
        m_anchor = new GameObject("Anchor");
        //set anchor to static point where mouse right clicked
        m_anchor.transform.position = new Vector3(tempMousePos.x, tempMousePos.y, 0.0f);
        //set the anchor
        balloon.SetAnchor(m_anchor, Vector3.Distance(m_anchor.transform.position, balloon.transform.GetChild(0).position));
    }

    //make GameObject point towards the mouse
    private void RotateTowardsMouse()
    {
        Transform rotatingPart = transform.Find("Pivot");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - rotatingPart.position.y, mousePos.x - rotatingPart.position.x) * Mathf.Rad2Deg;
        rotatingPart.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        if (CompareTag("Fan"))
        {
            if (gameObject.transform.Find("Pivot").GetComponent<CapsuleCollider2D>().IsTouchingLayers())
            {
                GameObject fanHead = transform.Find("Pivot").transform.Find("fanHead").gameObject;
                fanHead.GetComponent<cakeslice.Outline>().color = 1;
            }
            else if (gameObject.transform.Find("fanBase").GetComponent<cakeslice.Outline>().color != 1)
            {
                GameObject fanHead = transform.Find("Pivot").transform.Find("fanHead").gameObject;
                fanHead.GetComponent<cakeslice.Outline>().color = 0;
            }
        }
    }

    private void RotateAllTowardsMouse()
    {
        Transform anchor = transform.Find("AnchorPoint");
        Transform ball = transform.Find("Ball");
        List<Transform> hinges = new List<Transform>();
        for (int index = 0; index < 4; index++)
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
            hinges[index].position = new Vector2(dist * (Mathf.Cos(angle * Mathf.Deg2Rad)), dist * (Mathf.Sin(angle * Mathf.Deg2Rad)));
            hinges[index].position += anchor.position;
            dist += 0.5f;
        }
        dist += 0.3f;
        ball.position = new Vector2(dist * (Mathf.Cos(angle * Mathf.Deg2Rad)), dist * (Mathf.Sin(angle * Mathf.Deg2Rad)));
        ball.position += anchor.position;
        List<Transform> children = new List<Transform>();
        for (int index = 0; index < transform.childCount; index++)
        {
            children.Add(transform.GetChild(index));
        }

        transform.DetachChildren();
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + 180.0f));
        foreach (Transform child in children)
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

    public void Init()
    {
        m_init = false;
        m_spawnPos = transform.position;

        m_overlapArray = new Collider2D[100];
        m_overlapFilter = new ContactFilter2D();
        m_overlapFilter.SetDepth(0.0f, 2.0f);

        m_gameControllerDrag = false;

        m_mouseDragStart = new Vector3(9999, 9999);
        if (GameObject.FindGameObjectWithTag("GameController") != null)
        {
            m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        m_outlineController = new List<cakeslice.Outline>(gameObject.GetComponentsInChildren<cakeslice.Outline>());
        m_rightClicked = false;

        if (!m_selected)
        {
            m_selected = false;
        }

        m_rb2 = gameObject.GetComponent<Rigidbody2D>();
        m_click = false;
        m_dragged = false;
        m_startPosition = transform.position;
    }

    /// <summary>
    /// Returns true if gameObject is currently overlapping with a different collider
    /// </summary>
    /// <returns>bool</returns>
    private bool ColliderOverlaps()
    {
        Physics2D.SyncTransforms();

        Array.Clear(m_overlapArray, 0, m_overlapArray.Length);

        var childrenColliders = gameObject.GetComponentsInChildren<Collider2D>();

        if (CompareTag("Fan"))
        {
            List<Collider2D> childList = new List<Collider2D>();

            foreach (var collider in childrenColliders)
            {
                if (collider != GetComponent<Collider2D>() && !collider.isTrigger)
                {
                    Collider2D[] tempArr = new Collider2D[100];
                    Physics2D.OverlapCollider(collider, m_overlapFilter, tempArr);

                    foreach (var tempCol in tempArr)
                    {
                        if (tempCol != null)
                        {
                            childList.Add(tempCol);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            m_overlapArray = childList.ToArray();
        }
        else
        {
            Physics2D.OverlapCollider(GetComponent<Collider2D>(), m_overlapFilter, m_overlapArray);
        }

        foreach (var collider in m_overlapArray)
        {
            if (collider != null)
            {
                if (collider.transform.root.gameObject == gameObject)
                {
                    continue;
                }
                return true;
            }
            else
            {
                break;
            }
        }
        return false;
    }

    private void ChangeOutlineColour(int t_colour)
    {
        if (m_outlineController != null)
        {
            foreach (var outline in m_outlineController)
            {
                outline.color = t_colour;
            }
        }
    }

    /// <summary>
    /// Function to enable/disable outline renderer.
    /// t_erase == true -> disables renderer
    /// t_erase == false -> enables renderer
    /// </summary>
    /// <param name="t_erase">Value to set the outline renderer</param>
    private void EnableOutlineRenderer(bool t_erase)
    {
        if (m_outlineController != null)
        {
            foreach (var outline in m_outlineController)
            {
                outline.eraseRenderer = t_erase;
            }
        }
    }

    private bool IsOutlineColour(int t_colour)
    {
        if (m_outlineController != null)
        {
            foreach (var outline in m_outlineController)
            {
                if (outline.color == t_colour)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
