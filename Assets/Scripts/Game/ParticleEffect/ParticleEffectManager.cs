using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffectManager : MonoBehaviour
{
    private void OnEnable() {
        ParticleSystem ps = this.GetComponent<ParticleSystem>();
        ps.Play();
    }
    private void OnDisable() {
        ParticleSystem ps = this.GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Clear();
    }
}
