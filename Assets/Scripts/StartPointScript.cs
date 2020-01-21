using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour
{
    private SpriteRenderer m_sprite;
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.transform.position = new Vector3(transform.position.x, transform.position.y - m_sprite.bounds.size.y);
    }

    public void Reset()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(transform.position.x, transform.position.y - m_sprite.bounds.size.y);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
