using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSoundSpawn : MonoBehaviour
{
    [SerializeField]
    EnemyMovement script;
    private void OnEnable()
    {
        if (GameController.Instance.GameIsStarted == true)
        {
            PlaySound();
        }       
    }

    private void PlaySound()
    {
        int[] index = { 25, 26, 27 };
        AudioManager.Instance.PlayRandomOneShot(index);
    }
}
