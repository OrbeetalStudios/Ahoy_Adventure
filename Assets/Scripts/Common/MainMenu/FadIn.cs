using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadIn : MonoBehaviour
{
    public void StartMusic()
    {
        AudioManager.Instance.StopSpecificOneShot(11);
        AudioManager.Instance.PlaySpecificMusic(0);
    }
}
