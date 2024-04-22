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
        musicGroup.audioMixer.SetFloat("MusicVolume", 0);
    }
    public void MusicOn()
    {
        musicGroup.audioMixer.SetFloat("MusicVolume", 100);
    }

    public void SoundOff()
    {
        oneShotGroup.audioMixer.SetFloat("MusicVolume", 0);
    }

    public void SoundOn()
    {
        oneShotGroup.audioMixer.SetFloat("MusicVolume", 100);
    }

    public void PlayBee()
    {
        
    }
}
