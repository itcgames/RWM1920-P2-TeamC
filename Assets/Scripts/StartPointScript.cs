using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour
{
    private GameObject m_player;
    private SpriteRenderer m_sprite;
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        m_player.transform.position = new Vector3(transform.position.x, transform.position.y - m_sprite.bounds.size.y);
    }
}
