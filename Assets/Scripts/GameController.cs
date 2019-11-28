using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool m_isSimRunning = false;
    public GameObject m_startSimButton;
    public GameObject m_resetButton;
    public GameObject m_stopSimButton;

    void Start()
    {
    }

    void Update()
    {
        if(m_isSimRunning)
        {
            Physics2D.autoSimulation = true;
            m_startSimButton.SetActive(false);
            m_resetButton.SetActive(false);
            m_stopSimButton.SetActive(true);
        }
        else
        {
            Physics2D.autoSimulation = false;
            m_startSimButton.SetActive(true);
            m_resetButton.SetActive(true);
            m_stopSimButton.SetActive(false);
        }
    }

    public void StartSim()
    {
        m_isSimRunning = true;
    }
    public void StopSim()
    {
        m_isSimRunning = false;
    }
}
