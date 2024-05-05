using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStart : MonoSingleton<WaveStart>
{
    public Animator anim;


    private void PlayAnimation()
    {
        anim = GetComponent<Animator>();
        anim.Play(0);
    }


}
