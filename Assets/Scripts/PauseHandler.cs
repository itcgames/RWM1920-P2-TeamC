using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public GameObject m_pausePanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            SwitchPausePanel();
        }
    }

    public void SwitchPausePanel()
    {
        m_pausePanel.SetActive(!m_pausePanel.activeSelf);
    }
}
