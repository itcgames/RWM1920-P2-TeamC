using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionButton : MonoBehaviour
{
    private UIController m_script;
    void Start()
    {
        m_script = transform.parent.parent.parent.gameObject.GetComponent<UIController>();
    }

    public void optionPressed()
    {
        m_script.optionButtonPressed();
    }
}
