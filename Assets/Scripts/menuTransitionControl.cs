using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuTransitionControl : MonoBehaviour
{
    private bool m_menuInView = true; //This bool controls if the menu buttons slide off screen. Used to remove the stuff to let the option screen come up.
    private Vector3 m_inView;
    private Vector3 m_outOfView;
    private int m_offset = 800;

    // Start is called before the first frame update
    void Start()
    {
        m_inView = transform.position;
        m_outOfView = m_inView;
        m_outOfView.x = m_outOfView.x + m_offset;
  
    }

    public void setTransition(bool t_transition)
    {
        m_menuInView = t_transition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_menuInView == false)
        {
            transitionOut();
        }
        else
        {
            transitionIn();
        }
    }

    void transitionOut()
    {
        if (transform.position.x < m_outOfView.x)
        {
            transform.position = transform.position + new Vector3(20, 0, 0);
        }
    }

    void transitionIn()
    {
        if (transform.position.x > m_inView.x)
        {
            transform.position = transform.position - new Vector3(20, 0, 0);
        }

    }
}
