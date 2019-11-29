using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool m_menuInView = true;
    private bool m_optionsInView = false;
    public void OptionButtonPressed()
    {
        
        m_menuInView = false;
        m_optionsInView = true;
        SetTransitions();
    }

    public void ExitOptionButtonPressed()
    {
        m_menuInView = true;
        m_optionsInView = false;
        SetTransitions();
    }

    void SetTransitions()
    {
        menuTransitionControl menuControl = transform.GetChild(0).GetComponent<menuTransitionControl>();
        menuControl.SetTransition(m_menuInView);

        optionTransitionControl optionControl = transform.GetChild(1).GetComponent<optionTransitionControl>();
        optionControl.SetTransition(m_optionsInView);
    }
}
