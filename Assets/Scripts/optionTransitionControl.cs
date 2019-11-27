using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionTransitionControl : MonoBehaviour
{
    private bool m_optionInView = false; //This bool controls if the Options buttons slide off screen. Used to remove the stuff to let the option screen come up.
    private Vector3 m_inView;
    private Vector3 m_outOfView;
    private int m_offset = 1000;


    // Start is called before the first frame update
    void Start()
    {
        m_inView = transform.position;
        m_outOfView = m_inView;
        m_outOfView.y = m_outOfView.y + m_offset;
        transform.position = m_outOfView;
    }

    public void setTransition(bool t_transition)
    {
        m_optionInView = t_transition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_optionInView == false)
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
        if (transform.position.y < m_outOfView.y)
        {
            transform.position = transform.position + new Vector3(0, 20, 0);
        }
    }

    void transitionIn()
    {
        if (transform.position.y > m_inView.y)
        {
            transform.position = transform.position - new Vector3(0, 20, 0);
        }
    }
}
