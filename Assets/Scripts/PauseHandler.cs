﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public GameObject m_pausePanel;
    public GameObject m_pauseButton;
    private bool m_paused;

    void Start()
    {
        m_paused = false;
        m_pausePanel.SetActive(false);
        m_pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

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
        m_paused = !m_paused;
        m_pausePanel.SetActive(!m_pausePanel.activeSelf);
        m_pauseButton.SetActive(!m_pauseButton.activeSelf);

        if (m_paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
