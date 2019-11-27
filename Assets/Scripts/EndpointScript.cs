using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndpointScript : MonoBehaviour
{
    public ParticleSystem m_particles;
    private bool m_playerTouching;
    void Start()
    {
        m_playerTouching = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name == "Player" || collision.gameObject.tag == "Player") && !m_playerTouching)
        {
            m_playerTouching = true;
            // popup window here
            m_particles.Play();
        }
    }
}
