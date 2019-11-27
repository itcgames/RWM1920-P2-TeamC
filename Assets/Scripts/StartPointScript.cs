using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour
{
    private GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {
        m_player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        m_player.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
