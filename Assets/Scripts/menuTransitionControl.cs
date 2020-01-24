using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuTransitionControl : MonoBehaviour
{
    private bool m_menuInView = true; //This bool controls if the menu buttons slide off screen. Used to remove the stuff to let the option screen come up.
    private Vector3 m_inView;
    private Vector3 m_outOfView;
    private float m_offset = Screen.height * 1.5f;
    private Vector2 m_resolution;
    private float m_transitionsRate = 20;//Move X  Pixels Per Second


    // Start is called before the first frame update
    void Start()
    {
        m_inView = transform.position;
        m_outOfView = m_inView;
        m_outOfView.y = m_outOfView.y - m_offset;
    }

    public void SetTransition(bool t_transition)
    {
        m_menuInView = t_transition;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_menuInView == false)
        {
            TransitionOut();
        }
        else
        {
            TransitionIn();
        }
    }

    void TransitionOut()
    {
        if (transform.position.y > m_outOfView.y)
        {
            transform.position = transform.position - new Vector3(0, m_transitionsRate, 0);
        }
    }

    void TransitionIn()
    {
        if (transform.position.y < m_inView.y)
        {
            transform.position = transform.position + new Vector3(0, m_transitionsRate, 0);
        }
    }
}
