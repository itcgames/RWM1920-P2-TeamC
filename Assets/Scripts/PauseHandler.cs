using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public GameObject puasePanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            switchPausePanel();
        }
    }

    public void switchPausePanel()
    {
        puasePanel.SetActive(!puasePanel.activeSelf);
    }
}
