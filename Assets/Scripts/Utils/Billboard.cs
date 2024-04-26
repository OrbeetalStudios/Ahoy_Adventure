using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform camera;

    private void Awake() {
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(new Vector3(camera.position.x, camera.position.y, 1000));
    }
}
