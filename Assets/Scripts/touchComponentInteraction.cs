//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class touchComponentInteraction : MonoBehaviour
//{
//    private bool m_clicked;
//    private bool m_selected;
//    private bool m_moving;
//    private List<cakeslice.Outline> m_outlineController;
//    // Start is called before the first frame update
//    void Start()
//    {
//        m_clicked = false;
//        m_moving = false;
//        m_selected = false;
//        m_outlineController = new List<cakeslice.Outline>(gameObject.GetComponentsInChildren<cakeslice.Outline>());
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(Input.GetMouseButtonDown(0) && !m_clicked && m_selected && !m_moving)
//        {
//            m_selected = false;
//            gameObject.GetComponentInChildren<TouchInterfaceBehaviour>().CancelButtonClicked();
//        }
//    }

//    private void OnMouseDown()
//    {
//        m_clicked = true;
//        if (!m_selected)
//        {
//            m_selected = true;
//            //for each outline script on this gameobject and its children
//            foreach (var outline in m_outlineController)
//            {
//                //enable outline drawing
//                outline.eraseRenderer = false;
//                outline.color = 3;
//            }
//            SpawnButtons();
//        }
//    }

//    private void OnMouseUp()
//    {
//        m_clicked = false;
//    }

//    //private void OnCollisionExit2D(Collision2D collision)
//    //{
//    //    if (m_selected)
//    //    {
//    //        foreach (var outline in m_outlineController)
//    //        {
//    //            outline.color = 0;
//    //        }
//    //    }
//    //}

//    //private void OnCollisionStay2D(Collision2D collision)
//    //{
//    //    if (m_selected)
//    //    {
//    //        foreach (var outline in m_outlineController)
//    //        {
//    //            outline.color = 1;
//    //        }
//    //    }
//    //}

//    private void SpawnButtons()
//    {
//        //GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

//        GameObject buttons = Instantiate(Resources.Load<GameObject>("Prefabs/TouchInterface"));
//        buttons.transform.SetParent(transform);
//    }

//    public void Deselect()
//    {
//        m_selected = false;
//        foreach (var outline in m_outlineController)
//        {
//            outline.eraseRenderer = true;
//        }
//    }

//    public void SetMoving(bool t_moving)
//    {
//        m_moving = t_moving;
//    }

//    public bool GetMoving()
//    {
//        return m_moving;
//    }
//}
