using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    public void LoadAScene(string t_scene)
    {
        SceneManager.LoadScene(t_scene, LoadSceneMode.Single);
    }
}
