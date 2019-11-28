using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour
{
    private GameObject m_player;
    void Start()
    {
        m_player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        m_player.transform.position = transform.position;
    }
}
