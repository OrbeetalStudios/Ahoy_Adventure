using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{

    [SerializeField]
    private GameObject On_MusicButton;
    [SerializeField]
    private GameObject Off_MusicButton;
    [SerializeField]
    private GameObject On_SoundButton;
    [SerializeField]
    private GameObject Off_SoundButton;

    public void Awake()
    {
        if (AudioManager.Instance.musicIsPlaying == true)
        {
            On_MusicButton.SetActive(true);
            Off_MusicButton.SetActive(false);
        }
        else
        {
            On_MusicButton.SetActive(false);
            Off_MusicButton.SetActive(true);
        }

        if (AudioManager.Instance.soundIsPlaying == true)
        {
            On_SoundButton.SetActive(true);
            Off_SoundButton.SetActive(false);
        }
        else
        {
            On_SoundButton.SetActive(false);
            Off_SoundButton.SetActive(true);
        }


    }
    public void OffMusic()
    {
        AudioManager.Instance.MusicOff();
        On_MusicButton.SetActive(false);
        Off_MusicButton.SetActive(true);
    }

    public void OnMusic()
    {
        AudioManager.Instance.MusicOn();
        On_MusicButton.SetActive(true);
        Off_MusicButton.SetActive(false);
    }

    public void OffSound()
    {
        AudioManager.Instance.SoundOff();
        On_SoundButton.SetActive(false);
        Off_SoundButton.SetActive(true);
    }

    public void OnSound()
    {
        AudioManager.Instance.SoundOn();
        On_SoundButton.SetActive(true);
        Off_SoundButton.SetActive(false);
    }
}
