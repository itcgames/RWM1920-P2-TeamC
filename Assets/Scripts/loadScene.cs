using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    private int m_totalLevels = 2;

    public void LoadAScene(string t_scene)
    {
        SceneManager.LoadScene(t_scene, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        string name = SceneManager.GetActiveScene().name;
        int last = (int)char.GetNumericValue(name[name.Length - 1]);

        if (last == m_totalLevels)
        {
            LoadAScene("mainMenu");
        }
        else
        {
            name = name.Remove(name.Length - 1);
            name = name + (last + 1);
            SceneManager.LoadScene(name);
        }
    }
}
