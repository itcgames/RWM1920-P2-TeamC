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
            switchPausePanel();
        }
    }

    public void switchPausePanel()
    {
        m_pausePanel.SetActive(!m_pausePanel.activeSelf);
    }
}
