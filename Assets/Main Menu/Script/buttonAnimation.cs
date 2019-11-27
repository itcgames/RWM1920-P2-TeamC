using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class buttonAnimation : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
	private bool mouseOver = false;
	private Vector3 startPos;
	private Vector3 offsetPos;
	private int offset = -20;//Change this to set the offset Amount.

    void Start(){
		startPos = transform.position;
		offsetPos = startPos;
		offsetPos.x = offsetPos.x + offset;

    }

	void Update(){



        if (mouseOver){//Moves to the right if the mouse is hovering, to the OffsetPos 
			handleAnimation();
		}
		else{//Else returns to the left to the startPos
			resetAnimation();
		}


    }

	public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area. It uses it's childs invisible larger area to determine this
        mouseOver = true;
        
	}

	public void OnPointerExit(PointerEventData eventData){//When the Mouse has exited the UI Element area. It uses it's childs invisible larger area to determine this

        mouseOver = false;
    }

	 void handleAnimation(){
		if(transform.position.x != offsetPos.x){
         
            transform.position = transform.position - new Vector3(1,0,0) ;
		}

	}
	 void resetAnimation(){
		if(transform.position.x < startPos.x){
			transform.position = transform.position + new Vector3(1,0,0) ;
		}

	}
}


