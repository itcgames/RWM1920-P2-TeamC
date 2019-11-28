using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdShadowAnimation : MonoBehaviour
{
    private Vector3 m_startPos;
    private Vector3 m_birdSpeed;
    private int m_offsetTime = 15;//The number of seconds before the bird is reset in seconds
    private int m_count = 0;
    void Start()
    {
        m_startPos = transform.position;
        m_birdSpeed = new Vector3( 50, -50, 0 );
        m_offsetTime = m_offsetTime * 60;
        
    }

    // Update is called once per frame
    void Update()
    {
        m_count++;
        transform.position = transform.position + m_birdSpeed;
        if (m_count >= m_offsetTime )
        {
            m_count = 0;
            transform.position = m_startPos;
        }
    }
}
