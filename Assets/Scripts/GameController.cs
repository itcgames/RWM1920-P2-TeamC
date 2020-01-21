﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool m_isSimRunning = false;
    public GameObject m_startSimButton;
    public GameObject m_resetButton;
    public GameObject m_stopSimButton;
    private bool m_wreckingBallReset;

    void Start()
    {
        m_wreckingBallReset = false;
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
        if (!m_wreckingBallReset)
        {
            ResetWreckingBalls();
        }
    }

    public void EnableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        foreach (Rigidbody2D obj in rb)
        {
            obj.constraints = RigidbodyConstraints2D.None;
        }
        GameObject[] wreckingBalls = GameObject.FindGameObjectsWithTag("WreckingBall");
        foreach (var ball in wreckingBalls)
        {
            ball.transform.Find("AnchorPoint").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        m_wreckingBallReset = false;
    }

    private void ResetWreckingBalls()
    {
        GameObject[] wreckingBalls = GameObject.FindGameObjectsWithTag("WreckingBall");
        foreach (var ball in wreckingBalls)
        {
            foreach (Transform child in ball.transform)
            {
                child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                float xPosition = 0;
                switch (child.tag)
                {
                    case "Ball":
                        xPosition = -2.8f;
                        break;
                    case "AnchorPoint":
                        xPosition = -0.2f;
                        break;
                    case "HingeOne":
                        xPosition = -0.5f;
                        break;
                    case "HingeTwo":
                        xPosition = -1.0f;
                        break;
                    case "HingeThree":
                        xPosition = -1.5f;
                        break;
                    case "HingeFour":
                        xPosition = -2.0f;
                        break;
                    default:
                        break;
                }
                Debug.Log(child.tag);
                child.localPosition = new Vector3(xPosition, 0.0f, 0.0f);
                child.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            }
        }
        m_wreckingBallReset = true;
        foreach (var ball in wreckingBalls)
        {
            foreach (Transform child in ball.transform)
            {
                Vector3 pos = child.position;
                Vector3 angle = child.eulerAngles;
            }
        }
    }
    
}
