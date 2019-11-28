using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class exitOptionButton : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
    private UIController m_script;
    void Start()
    {
        m_script = transform.parent.parent.gameObject.GetComponent<UIController>();
    }

    public void OptionPressed()
    {
        m_script.ExitOptionButtonPressed();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {//When the Mouse is over the UI Element area

        transform.localScale = new Vector2(1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {//When the Mouse has exited the UI Element area
        transform.localScale = new Vector2(1f, 1f);
    }
}
