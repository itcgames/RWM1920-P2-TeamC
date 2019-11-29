using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallFreezeBehaviour : MonoBehaviour
{

    [Tooltip("Check Whether Wrecking Ball Should Be Frozen ")]
    public bool m_freeze;

    private bool m_hasBeenFrozen;
    private List<Rigidbody2D> m_childrenRigidbodies;
    private List<Vector3> m_startPositions;
    private List<Vector3> m_startRotations;

    void Start()
    {
        m_hasBeenFrozen = false;
        Debug.Log(transform.childCount);
        m_childrenRigidbodies = new List<Rigidbody2D>();
        m_startRotations = new List<Vector3>();
        m_startPositions = new List<Vector3>();
        for (int index = 0; index < transform.childCount; index++)
        {
            m_childrenRigidbodies.Add(transform.GetChild(index).gameObject.GetComponent<Rigidbody2D>());

            m_startPositions.Add(transform.GetChild(index).position);
            m_startRotations.Add(transform.GetChild(index).rotation.eulerAngles);
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
        else if (!m_freeze && m_hasBeenFrozen)
        {
             UnFreezeChildren();
        }
    }

    private void FreezeChildren()
    {
        for (int index = 0; index < transform.childCount; index++)
        {
            transform.GetChild(index).position = m_startPositions[index];
            transform.GetChild(index).eulerAngles = (m_startRotations[index]);
            //  transform.rotation = Quaternion.Euler(new Vector3());


   
        }
        foreach (Rigidbody2D rigidbody in m_childrenRigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        m_hasBeenFrozen = true;

        List<Transform> children = new List<Transform>();
        for (int index = 0; index < transform.childCount; index++)
        {
            children.Add(transform.GetChild(index));
        }

        transform.DetachChildren();
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        foreach (Transform child in children)
        {
            child.parent = transform;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -transform.GetChild(1).eulerAngles.z));
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
