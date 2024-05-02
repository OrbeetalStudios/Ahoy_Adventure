using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform camera;

    private void Awake() {
        camera = Camera.main.transform;
        Timing.RunCoroutine(BillCoroutine().CancelWith(gameObject));
    }

    protected IEnumerator<float> BillCoroutine()
    {
        while (isActiveAndEnabled)
        {
            transform.LookAt(new Vector3(camera.position.x, camera.position.y, 1000));
            yield return Timing.WaitForOneFrame;
        }
    }

}
