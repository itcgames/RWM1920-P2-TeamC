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
        DisableObjects();
    }

    void Update()
    {
        if(!m_isSimRunning)
        {
            DisableObjects();
        }
        else
        {
            EnableObjects();
        }
        //Physics2D.autoSimulation = m_isSimRunning;
        m_startSimButton.SetActive(!m_isSimRunning);
        m_resetButton.SetActive(!m_isSimRunning);
        m_stopSimButton.SetActive(m_isSimRunning);
    }

    public void StartSim()
    {
        m_isSimRunning = true;
    }
    public void StopSim()
    {
        m_isSimRunning = false;
    }
    public bool IsSimRunning()
    {
        return m_isSimRunning;
    }

    public void DisableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        foreach (Rigidbody2D obj in rb)
        {
            obj.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void EnableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        foreach (Rigidbody2D obj in rb)
        {
            obj.constraints = RigidbodyConstraints2D.None;
        }
    }
}
