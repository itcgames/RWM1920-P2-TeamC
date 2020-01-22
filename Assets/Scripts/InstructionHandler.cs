using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentPage = 1;
    private int numberOfPages = 6;

    private bool m_menuInView = OptionsData.SeenInstructions; //This bool controls if the menu buttons slide off screen. Used to remove the stuff to let the option screen come up.
    private Vector3 m_inView;
    private Vector3 m_outOfView;
    private int m_offset = 1000;
    private float m_transitionsRate = 50;//Move X  Pixels Per Second


    void Start()
    {
        m_inView = transform.position;
        m_outOfView = m_inView;
        m_outOfView.y = m_outOfView.y - m_offset;
        this.transform.position = m_outOfView;
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

    public void SetTransition(bool t_transition)
    {
        m_menuInView = t_transition;
        if(OptionsData.SeenInstructions == true)
        {
            OptionsData.SeenInstructions = false;
        }
    }

    public void NextPage()
    {
        transform.GetChild(currentPage).gameObject.SetActive(false);
        if (currentPage < numberOfPages)
        {
            currentPage++;
            transform.GetChild(currentPage).gameObject.SetActive(true);
        }
        if(currentPage == numberOfPages)
        {
            currentPage = 1;
            transform.GetChild(currentPage).gameObject.SetActive(true);
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
