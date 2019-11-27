using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class buttonAnimation : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
	private bool m_mouseOver = false;
	private Vector3 m_startPos;
	private Vector3 m_offsetPos;
	private int m_offset = -20;//Change this to set the offset Amount.

    void Start(){
        m_startPos = transform.position;
		m_offsetPos = m_startPos;
		m_offsetPos.x = m_offsetPos.x + m_offset;

    }

	void Update(){



        if (m_mouseOver){//Moves to the right if the mouse is hovering, to the OffsetPos 
			handleAnimation();
		}
		else{//Else returns to the left to the startPos
			resetAnimation();
		}


    }

	public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area. It uses it's childs invisible larger area to determine this
        m_mouseOver = true;
        
	}

	public void OnPointerExit(PointerEventData eventData){//When the Mouse has exited the UI Element area. It uses it's childs invisible larger area to determine this

        m_mouseOver = false;
    }

	 void handleAnimation(){
		if(transform.position.x != m_offsetPos.x){
         
            transform.position = transform.position - new Vector3(1,0,0) ;
		}

	}
	 void resetAnimation(){
		if(transform.position.x < m_startPos.x){
			transform.position = transform.position + new Vector3(1,0,0) ;
		}

	}
}


