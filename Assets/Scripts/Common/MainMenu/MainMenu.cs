using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject crew;
    private bool OpenSettings = false;
    private bool OpenCrew = false;
    public GameObject fadeOut;


    public void PlayGame()
    {
        fadeOut.SetActive(true);
        SceneManager.LoadScene(2);
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

    public void OpenCrewPanel()
    {
        if (!OpenCrew)
        {
            OpenCrew = true;
            crew.SetActive(true);
        }
        else
        {
            OpenCrew = false;
            crew.SetActive(false) ;
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
