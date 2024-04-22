using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager> 
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioMixerGroup musicGroup, oneShotGroup;
    [SerializeField]
    private List<AudioSource> Musics;
    [SerializeField]
    private List<AudioSource> OneShots;

    public void MusicOff()
    {
        musicGroup.audioMixer.SetFloat("MusicVolume", -80f);
    }
    public void MusicOn()
    {
        musicGroup.audioMixer.SetFloat("MusicVolume", -14.03f);
    }

    public void SoundOff()
    {
        oneShotGroup.audioMixer.SetFloat("OneShotsVolume", -80f);
    }

    public void SoundOn()
    {
        oneShotGroup.audioMixer.SetFloat("OneShotsVolume", -14.03f);
    }

    public void PlaySpecificMusic(int index)
    {
        if (index >= 0 && index < Musics.Count)
        {
            Musics[index].Play();
        }
        else
        {
            Debug.LogWarning("Invalid index for music file.");
        }
    }
    public void StopSpecificMusic(int index)
    {
        if (index >= 0 && index < Musics.Count)
        {
            Musics[index].Stop();
        }
        else
        {
            Debug.LogWarning("Invalid index for music file.");
        }
    }

    public void PlaySpecificOneShot(int index)
    {
        if (index >= 0 && index < OneShots.Count)
        {
            OneShots[index].Play();
        }
        else
        {
            Debug.LogWarning("Invalid index for one-shot file.");
        }
    }
    public void StopSpecificOneShot(int index)
    {
        if (index >= 0 && index < OneShots.Count)
        {
            OneShots[index].Stop();
        }
        else
        {
            Debug.LogWarning("Invalid index for one-shot file.");
        }
    }

}
