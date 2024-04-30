using MEC;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IDataPeristence
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
    public List<int> idPurchased = new List<int>();
    public List<int> idAssigned= new List<int>();
    public int doubloons;

    public void Start()
    {
        AudioManager.Instance.PlaySpecificMusic(0);
        DataPersistenceManager.instance.NowLoad();
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
            buttonPanel.SetActive(false);
            OpenSettings = true;

        }
        else
        {
            OpenSettings = false;
            buttonPanel.SetActive(true);
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
            DataPersistenceManager.instance.NowSave();

        }
    }

    public void OpenCredits()
    {

        creditsPanel.SetActive(true);
        exitButtonCredits.SetActive(true);  
        anim.Play("FadeInCredits", 0);
    }
    public void ExitCredits()
    {
        exitButtonCredits.SetActive(false); 
        anim.Play("FadOutCredits",0);
    }

    public void ExitSettings()
    {
        settings.SetActive(false);
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

    public void LoadData(GameData data)
    {
       idPurchased=data.idPurchased;    
       idAssigned=data.idAssigned;
       doubloons=data.doubloons;
    }

    public void SaveData(ref GameData data)
    {
        data.idPurchased = idPurchased;
        data.idAssigned = idAssigned;
        data.doubloons = doubloons;
    }
}
