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
        Debug.Log(m_isSimRunning);
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
        GameObject.FindGameObjectWithTag("Start").GetComponent<StartPointScript>().Reset();
        m_isSimRunning = false;
    }
    public bool IsSimRunning()
    {
        return m_isSimRunning;
    }

    public void DisableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        var wreckingBalls = GameObject.FindGameObjectsWithTag("WreckingBall");
        foreach (Rigidbody2D obj in rb)
        {
            obj.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        foreach  (var ball in wreckingBalls)
        {
            ball.GetComponent<WreckingBallFreezeBehaviour>().m_freeze = true;
        }
    }

    public void EnableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        foreach (Rigidbody2D obj in rb)
        {
            obj.constraints = RigidbodyConstraints2D.None;
        }
        var wreckingBalls = GameObject.FindGameObjectsWithTag("WreckingBall");

        foreach (var ball in wreckingBalls)
        {
            ball.GetComponent<WreckingBallFreezeBehaviour>().m_freeze = false;
        }
    }
}
