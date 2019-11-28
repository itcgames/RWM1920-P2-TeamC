using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallFreezeBehaviour : MonoBehaviour
{

    [Tooltip("Check Whether Wrecking Ball Should Be Frozen ")]
    public bool m_freeze;

    private bool m_hasBeenFrozen;
    private List<Rigidbody2D> m_childrenRigidbodies;


    void Start()
    {
        m_hasBeenFrozen = false;
        Debug.Log(transform.childCount);
        m_childrenRigidbodies = new List<Rigidbody2D>();
        for(int index = 0;index < transform.childCount; index++)
        {
            m_childrenRigidbodies.Add(transform.GetChild(index).gameObject.GetComponent<Rigidbody2D>());
        }
        if (m_freeze) FreezeChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_freeze && !m_hasBeenFrozen)
        {
            FreezeChildren();
        }
        else if(!m_freeze && m_hasBeenFrozen)
        {
            UnFreezeChildren();
        }
    }

    private void FreezeChildren()
    {
        foreach (Rigidbody2D rigidbody in m_childrenRigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        m_hasBeenFrozen = true;
    }

    private void UnFreezeChildren()
    {
        foreach (Rigidbody2D rigidbody in m_childrenRigidbodies)
        {
            rigidbody.constraints = rigidbody.name == "AnchorPoint" ? RigidbodyConstraints2D.FreezePosition : RigidbodyConstraints2D.None;
        }
        m_hasBeenFrozen = false;
    }
}
