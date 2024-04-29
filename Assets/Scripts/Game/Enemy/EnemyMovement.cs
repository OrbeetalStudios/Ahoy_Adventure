using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Collections;
using UnityEngine.Timeline;

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
                StartCoroutine(FadeOut());
            }
            if (distanceTraveled >= distanceThreshold)
            {
                distanceTraveled = 0;
                gameObject.SetActive(false);
                break;
            }
            yield return Timing.WaitForOneFrame;
        }
    }
    protected override IEnumerator<float> Move()
    {
        foreach(Material mat in meshRenderer.materials){
            ResetMaterial(mat);
        }
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

    IEnumerator FadeOut()
    {   
        float time = 0f;
        foreach (Material material in meshRenderer.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            // material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }
        while(distanceTraveled < distanceThreshold)
        {
            float t = distanceTraveled/distanceThreshold;
            foreach (Material material in meshRenderer.materials){
                material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(1.0f, 0.0f, time/10));
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    void ResetMaterial(Material material){
            material.SetInt("_SrcBlend", 1);
            material.SetInt("_DstBlend", 0);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.color = new Color(material.color.r, material.color.r, material.color.r, 1.0f);

            material.DisableKeyword("_ALPHABLEND_ON");
    }

}