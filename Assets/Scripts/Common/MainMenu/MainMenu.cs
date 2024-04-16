using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    private bool OpenSettings = false;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        if (OpenSettings == false)
        {
            settings.SetActive(true);
            OpenSettings = true;
        }
        else
        {
            OpenSettings = false;
            settings.SetActive(false);
        }
        
    }

    public void ExitSettings()
    {
        settings.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
