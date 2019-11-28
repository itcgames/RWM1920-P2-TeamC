using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool m_menuInView = true;
    private bool m_optionsInView = false;
    void Start()
    {
        
    }


    public void optionButtonPressed()
    {
        m_menuInView = false;
        m_optionsInView = true;
        setTransitions();
   
    }

    public void exitOptionButtonPressed()
    {
        m_menuInView = true;
        m_optionsInView = false;
        setTransitions();
        Debug.Log("HERE");

    }

    void setTransitions()
    {

        menuTransitionControl menuControl = transform.GetChild(1).GetComponent<menuTransitionControl>();
        menuControl.setTransition(m_menuInView);

        optionTransitionControl optionControl = transform.GetChild(2).GetComponent<optionTransitionControl>();
        optionControl.setTransition(m_optionsInView);




    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
