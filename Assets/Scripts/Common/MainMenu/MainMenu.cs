using MEC;
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


    public void Start()
    {
      
        AudioManager.Instance.PlaySpecificMusic(0);
    }

   
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
            AudioManager.Instance.StopSpecificMusic(0);
            AudioManager.Instance.PlaySpecificMusic(1);
            crew.SetActive(true);
        }
        else
        {
            OpenCrew = false;
            AudioManager.Instance.StopSpecificMusic(1);
            AudioManager.Instance.PlaySpecificMusic(0);
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

    public void OnClickCharacter()
    {
        AudioManager.Instance.PlaySpecificOneShot(0);
    }
    public void OnClickSound()
    {
        AudioManager.Instance.PlaySpecificOneShot(4);
    }

    public void PlayOnOff()
    {
        AudioManager.Instance.PlaySpecificOneShot(14);
    }

    public void ScrollMenu()
    {
        AudioManager.Instance.PlaySpecificOneShot(13);
    }

    public void HireSound()
    {
        AudioManager.Instance.PlaySpecificOneShot(10);
    }

}
