using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class buttonAnimation : MonoBehaviour
{
	private Vector3 m_startPos;
	private Vector3 m_offsetPos;
	private int m_offset = -20;//Change this to set the offset Amount.
    private  mousePresent m_script;
    
    void Start()
    {
        m_startPos = transform.position;
		m_offsetPos = m_startPos;
		m_offsetPos.x = m_offsetPos.x + m_offset;
        m_script = transform.parent.gameObject.GetComponent<mousePresent>();
    }

	void Update()
    {
        if (m_script.GetMousePresent())
        {//Moves to the right if the mouse is hovering, to the OffsetPos 
        	HandleAnimation();
        }
        else
        {//Else returns to the left to the startPos
        	ResetAnimation();
        }
    }
    
    void HandleAnimation()
    {
		if(transform.position.x != m_offsetPos.x)
        {        
            transform.position = transform.position - new Vector3(5,0,0) ;
		}
    }
    
    void ResetAnimation()
    {
		if(transform.position.x < m_startPos.x)
        {
			transform.position = transform.position + new Vector3(5,0,0) ;
		}
	}
}


