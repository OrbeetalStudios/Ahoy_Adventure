using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Collections;
using UnityEngine.Timeline;
using Unity.VisualScripting;
using System.Linq.Expressions;
using System;

public class EnemyMovement : AbstractMotionToTarget
{
    [SerializeField] protected float distanceTraveled;
    [SerializeField] protected float distanceThreshold = 150f;
    [SerializeField] private float startFadeOutDistance = 100f;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float initialAlpha = 1.0f;
    private bool isExitingMap = false;

    protected void OnEnable()
    {
        isExitingMap = false;
        moveToTargetHandle = Timing.RunCoroutine(Move().CancelWith(gameObject));
        try{
            PlayVFX(gameObject, PoolController.Instance.GetObjectFromCollection(EPoolObjectType.enemy_spawn_vfx));
        }catch(Exception e){
            // Debug.Log("");
        }
    }
    protected IEnumerator<float> ReturnOutsideMap(Vector3 relativePos)
    {
        isExitingMap = true;
        while (this.isActiveAndEnabled)
        {
            transform.position += speed * Time.deltaTime * relativePos.normalized;
            distanceTraveled = (transform.position - Vector3.zero).magnitude;
            if(isExitingMap && distanceTraveled >= startFadeOutDistance){
                isExitingMap = false;
                // StartCoroutine(FadeOut());
            }
            if (distanceTraveled >= distanceThreshold)
            {
                distanceTraveled = 0;
                gameObject.SetActive(false);
                PlayVFX(gameObject, PoolController.Instance.GetObjectFromCollection(EPoolObjectType.enemy_spawn_vfx));
                break;
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    protected override IEnumerator<float> Move()
    {
        while (true)
        {
            // relative vector from center to object
            Vector3 relativePos = transform.position - Vector3.zero;

            // Align rotation to radius direction vector, in order to always face the center object
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;

            // Update position
            transform.position -= (MultiplierAllowed ? speed * WavesController.Instance.WaveSpeedMultiplier : speed) * Time.deltaTime * relativePos.normalized;
            yield return Timing.WaitForOneFrame;
        }
    }

    private void PlayVFX(GameObject parent, GameObject effect){
        effect.transform.position = parent.transform.position;
        effect.SetActive(true);
    }
    private void OnDisable()
    {
        Timing.KillCoroutines("Move");
        Timing.KillCoroutines("ReturnOutsideMap");
    }
}