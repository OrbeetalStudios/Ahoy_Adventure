using MEC;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject buttonPanel;
    [SerializeField]
    private GameObject crew;
    private bool OpenSettings = false;
    private bool OpenCrew = false;
    public GameObject fadeOut;
    [SerializeField]
    private GameObject creditsPanel;
    public Animator anim;
    [SerializeField]
    private GameObject exitButtonCredits;
    public GameObject buttonSettings;
    public GameObject page;
    private float target;

    public void Start()
    {
        
        AudioManager.Instance.PlaySpecificMusic(0);
        DataPersistenceManager.instance.NowLoad();
        CrewController.Instance.SetCrewData();
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
            LeanTween.rotateY(buttonSettings, 90, 0.5f).setEase(LeanTweenType.easeOutBounce);
            OnClickSound();
        }
        else
        {
            settings.SetActive(false);
            OpenSettings = false;
            LeanTween.rotateY(buttonSettings, 0f, 0.5f).setEase(LeanTweenType.easeInBounce);
            OnClickSound();
        }       
    }
    public void OpenCrewPanel()
    {
        if (!OpenCrew)
        {
            OpenCrew = true;
            OnOpenCrew(crew);
            AudioManager.Instance.StopSpecificMusic(0);
            AudioManager.Instance.PlaySpecificMusic(1);
            DataPersistenceManager.instance.NowLoad();
          
        }
        else
        {
            OpenCrew = false;
            OnCloseCrewPanel(crew);
            AudioManager.Instance.StopSpecificMusic(1);
            AudioManager.Instance.PlaySpecificMusic(0);
            DataPersistenceManager.instance.NowSave();
            CrewController.Instance.SetCrewData();
        }
    }
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        exitButtonCredits.SetActive(true);  
    }
    public void ExitCredits()
    {
        exitButtonCredits.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
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

    public void OnOpenWindow(GameObject panel)
    {
        LeanTween.scale(panel, new Vector3(1f,1f,1f),0.8f).setDelay(.3f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void OnCloseCrewPanel(GameObject panel)
    {
        LeanTween.scale(panel, new Vector3(0f, 0f, 0f), 0.1f).setDelay(.3f).setEase(LeanTweenType.easeOutSine);
    }

   public void OnOpenCrew(GameObject panel)
    {
        LeanTween.scale(panel, new Vector3(1f, 1f, 1f), 0.1f).setDelay(.3f).setEase(LeanTweenType.easeInOutSine);
    }

}
