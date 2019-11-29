using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class SnailAnimation : MonoBehaviour
, IPointerClickHandler
{

    private Vector3 m_startPos;
    private Vector3 m_speedUp;
    private Vector3 m_speed;
    public AudioClip m_tapSFX;
    private AudioSource m_source;
    private float m_offset = 3000;
    private bool m_headingLeft = false;
    private int m_panicCount = 0;
    private int m_panicCap = 60;
    private int m_speedCap = 3;

    void Start()
    {
        m_startPos = transform.position;
        m_speed = new Vector3(2, 0, 0);
        m_source = transform.parent.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePanic();
        transform.position = transform.position + (m_speed + m_speedUp);
        if (Math.Abs(m_startPos.x - transform.position.x) > m_offset)
        {
            m_speed = m_speed * -1;
            m_speedUp = m_speedUp * -1;
            m_headingLeft = !m_headingLeft;

            Vector3 tempScale = transform.localScale;
            tempScale.x = tempScale.x * -1;
            transform.localScale = tempScale;
            m_startPos.x = transform.position.x;
            transform.position = new Vector3(m_startPos.x, m_startPos.y + UnityEngine.Random.Range(-500.0f, 500.0f), m_startPos.z);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area
        m_source.PlayOneShot(m_tapSFX);
        if (m_headingLeft)
        {
            m_speedUp.x = -m_speedCap;
        }
        else
        {
            m_speedUp.x = m_speedCap;
        }
        m_panicCount = m_panicCap;
    }


    void HandlePanic()
    {
        if (m_panicCount == 0)
        {

            if (m_speedUp.x > 0.01f)
            {
                float tempDecelleration = m_speedCap / 30f;
                m_speedUp = new Vector3 (m_speedUp.x - tempDecelleration,0,0);

            }
            else
            {
                m_speedUp.x = 0.01f;
            }
        }
        else
        {
            m_panicCount--;
            
        }
    }
}
