using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool m_isSimRunning = false;
    public GameObject m_startSimButton;
    public GameObject m_resetButton;
    public GameObject m_stopSimButton;
    private bool m_wreckingBallReset;

    private List<GameObject> m_addedBalloons;

    void Start()
    {
        m_addedBalloons = new List<GameObject>();
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
        m_startSimButton.SetActive(!m_isSimRunning);
        m_resetButton.SetActive(!m_isSimRunning);
        m_stopSimButton.SetActive(m_isSimRunning);
    }

    public void StartSim()
    {
        m_addedBalloons.Clear();
        var balloons = GameObject.FindGameObjectsWithTag("Balloon");

        foreach (var balloon in balloons)
        {
            var newBalloon = Instantiate(balloon);
            newBalloon.name = "UserPlacedBalloon";
            newBalloon.SetActive(false);
            m_addedBalloons.Add(newBalloon);
        }

        m_isSimRunning = true;
    }
    public void StopSim()
    {
        GameObject.FindGameObjectWithTag("Start").GetComponent<StartPointScript>().Reset();

        var oldBalloons = GameObject.FindGameObjectsWithTag("Balloon");
        for (int i = oldBalloons.Length - 1; i >= 0; i--)
        {
            Destroy(oldBalloons[i]);
        }
        foreach (var newBalloon in m_addedBalloons)
        {
            newBalloon.SetActive(true);
        }
        m_addedBalloons.Clear();

        m_isSimRunning = false;
    }
    public bool IsSimRunning()
    {
        return m_isSimRunning;
    }

    public void DisableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        
        foreach (Rigidbody2D obj in rb)
        {
            obj.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (!m_wreckingBallReset)
        {
            ResetWreckingBalls();
        }

        foreach(GameObject cannon in cannons)
        {
            cannon.GetComponent<FireObject>().fireOnContact = false;
        }

        foreach (GameObject portal in portals)
        {
            if (portal.transform.GetChild(0).name == "2D Collider")
            {
                portal.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void EnableObjects()
    {
        Rigidbody2D[] rb = Rigidbody2D.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        
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

        foreach (GameObject cannon in cannons)
        {
            cannon.GetComponent<FireObject>().fireOnContact = true;
        }

        foreach (GameObject portal in portals)
        {
            if (portal.transform.GetChild(0).name == "2D Collider")
            {
                portal.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void ResetWreckingBalls()
    {
        GameObject[] wreckingBalls = GameObject.FindGameObjectsWithTag("WreckingBall");
        foreach (var ball in wreckingBalls)
        {
            foreach (Transform child in ball.transform)
            {
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
                child.localPosition = new Vector3(xPosition, 0.0f, 0.0f);
                child.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
        m_wreckingBallReset = true;
    }
    
}
