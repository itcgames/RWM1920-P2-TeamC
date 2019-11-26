using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class buttonAnimation : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
	private bool mouse_over = false;
	private bool headingLeft = true;
	private Vector3 startPos;
	private Vector3 offsetPos;
	private int offset = 20;
	private int secondsMoving = 1;

	void Start(){
		startPos = transform.position;
		offsetPos = startPos;
		offsetPos.x = offsetPos.x + offset;
	}

	void Update(){
		if (mouse_over){
			handleAnimation();
		}
		else{
			resetAnimation();
		}

	}

	public void OnPointerEnter(PointerEventData eventData){
		mouse_over = true;
	}

	public void OnPointerExit(PointerEventData eventData){
		mouse_over = false;
	}

	 void handleAnimation(){
		if(transform.position.x < offsetPos.x){
			transform.position = transform.position + new Vector3(1,0,0) ;//(offset/(60*secondsMoving), 0, 0);
			Debug.Log("Hi");
		}

	}
	 void resetAnimation(){
		if(transform.position.x > startPos.x){
			//GameObject.transform.position.x += offset/(60*secondsMoving);
			transform.position = transform.position - new Vector3(1,0,0) ;//(offset/(60*secondsMoving), 0, 0);
		}

	}
}


