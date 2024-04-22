using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{

    [SerializeField]
    private GameObject OnButton;
    [SerializeField]
    private GameObject OffButton;

    public void OffMusic()
    {
        AudioManager.Instance.MusicOff();
        OnButton.SetActive(false);
        OffButton.SetActive(true);
    }

    public void OnMusic()
    {
        AudioManager.Instance.MusicOn();
        OffButton.SetActive(false);
        OnButton.SetActive(true);
    }

    public void OffSound()
    {
        AudioManager.Instance.SoundOff();
        OnButton.SetActive(false);
        OffButton.SetActive(true);
    }

    public void OnSound()
    {
        AudioManager.Instance.SoundOn();
        OffButton.SetActive(false);
        OnButton.SetActive(true);
    }
}
