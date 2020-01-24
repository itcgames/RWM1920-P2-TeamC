using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ButtonClickBehaviour;

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

    private TouchInterfaceBehaviour buttons;


    private bool cancelButtonPressed;
    private bool deleteButtonPressed;
    private bool moveNotRotate;
    private bool m_updateBallonAnchor;

    private Vector3 m_lastPos;

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
        if (deleteButtonPressed)
        {
            HandleDeleteButtonPressed();
        }
        else if (cancelButtonPressed)
        {
            HandleCancelButtonPressed();
        }


        //if selected and GameController is not null
        if (m_selected && m_controller != null)
        {
            //set selected to false if sim is running
            m_selected = !m_controller.IsSimRunning();
            //check if selected was set to false
            if (!m_selected)
            {
                UnselectComponent();
            }
        }
        //if selected
        if (m_selected)
        {
            if (moveNotRotate)
            {
                // long click/drag mouse
                if (m_dragged || m_click && (Time.time - m_mouseDownTime) >= LONG_CLICK_TIME)
                {
                    MoveComponent();
                }
            }
            else
            { 
            //if holding down RMB
            if (Input.GetMouseButtonDown(0))
            {
                //alter component depending on component typ
                HandleComponentAlteration();
            }

                //if we right clicked
                if (m_rightClicked)
                {
                    //check if this gameObject has a Fan or Cannon tag
                    if (CompareTag("Fan") || CompareTag("Cannon"))
                    {
                        //fan and cannon can use same rotate function
                        RotateTowardsMouse();
                    }
                    //if this gameObject has a WreckingBall tag
                    else if (CompareTag("WreckingBall"))
                    {
                        //handle rotating differently
                        RotateAllTowardsMouse();
                    }
                }
            }
        }

        ////if we clicked AND didnt click on component AND are selected
        //if (Input.GetMouseButtonDown(0) && !m_click && m_selected)
        //{
        //    //means we clicked off of the component, there unselect
        //    UnselectComponent();
        //}

        //check if this gameObject's colliders are overlapping with anything
        if (!ColliderOverlaps())
        {
            //if not colliding we can change the outline colour
            //and if we unselected, then outline is not visible
            ChangeOutlineColour(SELECTED_COLOUR);
            //update last position as we are not colliding
            m_lastPos = transform.position;
        }
        //otherwise we are colliding with something
        else
        {
            //change colour to Obstructed colour
            ChangeOutlineColour(OBSTRUCTED_COLOUR);
        }
        FindObjectOfType<MoveButtonBehaviour>().UpdateSpriteType(moveNotRotate);
    }

    void MoveComponent()
    {
        //if this gameObject has a rigidBody
        if (m_rb2 != null)
        {
            //make the rigidBody asleep temporarily
            m_rb2.Sleep();
            //if it is NOT a static body
            if (m_rb2.bodyType != RigidbodyType2D.Static)
            {
                //reset velocity
                m_rb2.velocity = Vector2.zero;
                //freeze rotation
                m_rb2.freezeRotation = true;
                //reset angular velocity
                m_rb2.angularVelocity = 0.0f;
            }
        }

        //get mouse position in game world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //move the gameObject to the mouse
        gameObject.transform.position = new Vector3(mousePos.x - m_clickStartPosX, mousePos.y - m_clickStartPosY, 0.0f);
    }

    private void OnMouseDrag()
    {
        //check if time since mouse LMB is smaller than what we consider a Long Click
        if ((Time.time - m_mouseDownTime) < LONG_CLICK_TIME)
        {
            //if we dragged long distance
            if (m_mouseDragStart.magnitude > 10000)
            {
                m_mouseDragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            //if drag start pos - currentMousePos is more than 0.0f
            if ((m_mouseDragStart - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)).magnitude > 0.0f)
            {
                //when object is being dragged by mouse, immiadiately consider it as a long click
                m_dragged = true;
                //and if this gameObecjt wasn't selected already, select it
                if (!m_selected)
                {
                    SelectComponent();
                }
            }
        }
        //otherwise we have a mouse long click, so if not selected
        //else if (!m_selected)
        //{
        //    //then select this gameObejct
        //    SelectComponent();
        //}
    }

    //if we hover over the object and RMB click
    private void OnMouseOver()
    {
        //if this gameObject is NOT selected and we RMB
        if (!m_selected && Input.GetMouseButtonDown(0))
        {
            //we have been rightClicked
            m_rightClicked = true;
        }
    }

    private void OnMouseDown()
    {
        //set time when we clicked to differiciate between short click and long click
        m_mouseDownTime = Time.time;
        SetClickStartPosOnObject();
        m_click = true;
        buttons = GameObject.FindGameObjectWithTag("TouchInterface").GetComponent<TouchInterfaceBehaviour>();
    }

    private void SpawnButtons()
    {
        GameObject buttons = Instantiate(Resources.Load<GameObject>("Prefabs/TouchInterface"));
        buttons.transform.SetParent(transform);

    }

    private void OnMouseUp()
    {
        //if this object has a 2d rigidbody
        if (m_rb2 != null)
        {
            //unfreeze it
            m_rb2.freezeRotation = false;
            //and wake it up if asleep
            if (m_rb2.IsSleeping())
            {
                m_rb2.WakeUp();
            }
        }

        //if time LMB was pressed down for less than what we consider a long click do stuff
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

    /// <summary>
    /// Function to Select this gameObject from a different script
    /// </summary>
    public void SelectFromGameController()
    {
        SelectComponent();
    }

    /// <summary>
    /// Function to Unselect this gameObject from a different script with an extra step
    /// </summary>
    public void UnselectFromGameController()
    {
        //check if we are trying to unselect while colliding
        if (ColliderOverlaps())
        {
            //if we were colliding, move back to last position where we didnt collide
            transform.position = m_lastPos;
        }
        //if we move the gameObject back to component spawn location, delete this object
        if (transform.position == m_spawnPos)
        {
            Destroy(gameObject);
        }

    }

    private void SelectComponent()
    {
        var components = FindObjectsOfType<ComponentInteraction>();
        bool canSelect = true;
        foreach (var compoenent in components)
        {
            if (compoenent.GetSelected())
            {
                canSelect = false;
            }
        }
        if (canSelect)
        {
            //select this object
            m_selected = true;
            //reset rightclick just in case
            m_rightClicked = false;

            //make the outline visible
            EnableOutlineRenderer(ENABLE_RENDERER);
        }
    }

    private void UnselectComponent()
    {
        //if gameObject is currently colliding
        if (IsOutlineColour(OBSTRUCTED_COLOUR))
        {
            //move back to safe position
            transform.position = m_lastPos;
        }
        //if we were OBSTRUCTED_COLOUR we can go back to SELECTED_COLOUR
        ChangeOutlineColour(SELECTED_COLOUR);

        //if we unselected while changing angle...
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
        if (cancelButtonPressed) cancelButtonPressed = false;
        moveNotRotate = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (m_selected)
        {
            foreach (var outline in m_outlineController)
            {
                outline.color = 0;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (m_selected)
        {
            foreach (var outline in m_outlineController)
            {
                outline.color = 1;
            }
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
                //if we're colliding
                if (IsOutlineColour(OBSTRUCTED_COLOUR))
                {
                    //move the rotating part back to safe location
                    transform.Find("Pivot").transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_originalAngle);
                    //we can set the colour back to SELECTED_COLOUR as the object should be in a safe location
                    ChangeOutlineColour(SELECTED_COLOUR);
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
        if (m_updateBallonAnchor)
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

            //check if any object with interactive script was right clicked
            foreach (var comp in interactiveComps)
            {
                //if it was...
                if (comp.m_rightClicked)
                {

                    //set anchor to that GameObject, with new anchor distance
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

            //If the collider overlaps the mouse coordinates
            if (player.GetComponent<Collider2D>().OverlapPoint(tempMousePos))
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

    /// <summary>
    /// Special magic function needed to rotate the wrecking ball
    /// </summary>
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


    public void HandleCancelButtonPressed()
    {
        UnselectComponent();
    }

    public void HandleDeleteButtonPressed()
    {
        Destroy(gameObject);
    }

    public void UpdateButtonPressed(string buttonName)
    {
        cancelButtonPressed = false;
        deleteButtonPressed = false;
        switch (buttonName)
        {
            case "MoveButton":
                moveNotRotate = !moveNotRotate;
                break;
            case "CancelButton":
                cancelButtonPressed = true;
                break;
            case "DeleteButton":
                deleteButtonPressed = true;
                break;
            default:
                break;
        }
    }

    public bool GetSelected()
    {
        return m_selected;
    }
    public void SetSelected(bool t_value)
    {
        m_selected = t_value;

        if (m_selected)
        {
            EnableOutlineRenderer(ENABLE_RENDERER);
        }
        else
        {
            EnableOutlineRenderer(DISABLE_RENDERER);
        }
    }
    /// <summary>
    /// Function to initialise this script
    /// </summary>
    public void Init()
    {
        m_init = false;
        m_spawnPos = transform.position;

        m_overlapArray = new Collider2D[10];
        m_overlapFilter = new ContactFilter2D();
        m_overlapFilter.SetDepth(0.0f, 2.0f);

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
        m_lastPos = transform.position;
        moveNotRotate = true;
        deleteButtonPressed = false;
        cancelButtonPressed = false;
        m_updateBallonAnchor = true;
    }

    /// <summary>
    /// Returns true if gameObject is currently overlapping with different colliders
    /// </summary>
    /// <returns>true if this gameObject's collider is overlapping</returns>
    private bool ColliderOverlaps()
    {
        //force a physics objects to update transforms
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
                    Collider2D[] tempArr = new Collider2D[10];
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

            //add items from the childList to the overLap array
            for (int i = 0; i < childList.Count; i++)
            {
                if (i < m_overlapArray.Length)
                {
                    m_overlapArray[i] = childList[i];
                }
                else
                {
                    break;
                }                
            }
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

    /// <summary>
    /// Change colour of the outline to passed in colour
    /// USE CONST INTS FROM THIS SCRIPT
    /// </summary>
    /// <param name="t_colour">colour to change the outline to</param>
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
    /// USE CONST BOOLS DECLARED FROM THIS SCRIPT
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

    /// <summary>
    /// Function to check if the current colour of the outline matches the passed in colour
    /// USE CONST INTS FROM THIS SCRIPT
    /// </summary>
    /// <param name="t_colour">colour to check</param>
    /// <returns>true if colour matches</returns>
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

    public void SetUpdateBallonAnchor(bool t_update)
    {
        m_updateBallonAnchor = t_update;
    }
}
