using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamEffect : MonoSingleton<FoamEffect>
{
    [SerializeField]
    private ParticleSystem foamEffect;

    private void OnEnable()
    {
        foamEffect.Stop();
    }

    public void StartFoam(bool isAnimStart)
    {
        if (isAnimStart == true)
        {
            foamEffect.Play();
        }
    }


}
