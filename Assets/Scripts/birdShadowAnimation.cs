using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class birdShadowAnimation : MonoBehaviour
, IPointerEnterHandler
{
    private Vector3 m_startPos;
    private Vector3 m_birdSpeed;
    private int m_offsetTime = 5;//The number of seconds before the bird is reset in seconds
    private int m_count = 0;
    public AudioClip m_birdSchreech;
    private AudioSource m_source;
    bool m_soundPlayed = false;

    void Start()
    {
        m_startPos = transform.position;
        m_birdSpeed = new Vector3( 50, -50, 0 );
        m_offsetTime = m_offsetTime * 60;
        m_source = gameObject.transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        m_count++;
        transform.position = transform.position + m_birdSpeed;
        if (m_count >= m_offsetTime )
        {
            m_count = 0;
            transform.position = new Vector3(m_startPos.x + Random.Range(-100.0f, 1000.0f), m_startPos.y, m_startPos.z) ;
            m_soundPlayed = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area

        if (m_soundPlayed == false)
        {
            m_source.PlayOneShot(m_birdSchreech);
            m_soundPlayed = true;
        }
    }
}
