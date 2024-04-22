using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{

    public GameObject plane;
  
   
    // Start is called before the first frame update
    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ActiveFlag()
    {
        
        plane.SetActive(true);
    }

    public void DeactiveFlag()
    {
        plane.SetActive(false);
    }

    public void PlayBee()
    {
        AudioManager.Instance.PlaySpecificOneShot(2);
    }

    public void StopBee()
    {
        AudioManager.Instance.StopSpecificOneShot(2);
    }

    public void PlayWave()
    {
        AudioManager.Instance.PlaySpecificOneShot(11);
    }

}

