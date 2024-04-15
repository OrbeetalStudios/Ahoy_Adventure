using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        settings.SetActive(true);
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
