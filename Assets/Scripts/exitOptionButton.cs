using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitOptionButton : MonoBehaviour
{
    private UIController script;
    void Start()
    {
        script = transform.parent.parent.gameObject.GetComponent<UIController>();

    }

    public void optionPressed()
    {
        script.exitOptionButtonPressed();
    }
}
