using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitButton: MonoBehaviour
{
    public void exitGame()
    {
        Debug.Log(" EXITING");
        Application.Quit();
        
    }
}
