using MEC;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    public Animator anim;
    [SerializeField]
    private GameObject exitButtonCredits;
    public GameObject buttonSettings;
    public GameObject page;
    public GameObject creditsPage;
    private float time;

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
            LeanTween.scale(buttonSettings, new Vector3(0f, 0f, 0f), 0.3f).setDelay(0f).setEase(LeanTweenType.easeOutElastic);
            OnClickSound();
        }
        else
        {
            settings.SetActive(false);
            OpenSettings = false;
            LeanTween.scale(buttonSettings, new Vector3(1f, 1f, 1f), 0.3f).setDelay(0f).setEase(LeanTweenType.easeInElastic);
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
            time = 0.05f;
            OnCloseCrewPanel(crew, time);
            AudioManager.Instance.StopSpecificMusic(1);
            AudioManager.Instance.PlaySpecificMusic(0);
            DataPersistenceManager.instance.NowSave();
            CrewController.Instance.SetCrewData();
        }
    }
    public void OpenCredits()
    {
        LeanTween.moveLocalY(creditsPage, -50f, 0.5f).setEase(LeanTweenType.easeInCubic);
        exitButtonCredits.SetActive(true);  
    }
    public void ExitCredits()
    {
        exitButtonCredits.SetActive(false);
        LeanTween.moveLocalY(creditsPage, 1000f, 0.5f).setEase(LeanTweenType.easeOutCubic);
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

    public void OnCloseCrewPanel(GameObject panel, float time)
    {
        LeanTween.scale(panel, new Vector3(0f, 0f, 0f), time).setDelay(time).setEase(LeanTweenType.easeOutSine);
    }

   public void OnOpenCrew(GameObject panel)
    {
        LeanTween.scale(panel, new Vector3(1f, 1f, 1f), 0.1f).setDelay(.3f).setEase(LeanTweenType.easeInOutSine);
    }

  

}
