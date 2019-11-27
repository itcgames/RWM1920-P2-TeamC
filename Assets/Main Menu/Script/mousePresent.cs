using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1

public class mousePresent : MonoBehaviour
{
    private Vector3 startPos;
    void Start()
    {

        startPos = transform.position;
    }

    void Update(){
        transform.position = startPos;

    }

	public void OnPointerEnter(PointerEventData eventData){//When the Mouse is over the UI Element area


    }

    public void OnPointerExit(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area

    }
}
