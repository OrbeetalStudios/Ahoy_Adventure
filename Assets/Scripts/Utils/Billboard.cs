using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
     Transform Cam;

    private void OnEnable() {
        Cam = Camera.main.transform;
        Timing.RunCoroutine(BillCoroutine().CancelWith(gameObject));
    }

    protected IEnumerator<float> BillCoroutine()
    {
        while (isActiveAndEnabled)
        {
            transform.LookAt(new Vector3(Cam.position.x, Cam.position.y, 1000));
            yield return Timing.WaitForOneFrame;
        }
    }

}
