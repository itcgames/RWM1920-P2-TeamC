using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionButton : MonoBehaviour
{
    private UIController script;
    void Start()
    {
        script = transform.parent.parent.parent.gameObject.GetComponent<UIController>();

    }

    public void optionPressed()
    {
        script.optionButtonPressed();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
