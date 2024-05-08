using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubloonsSOund : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlaySpecificOneShot(28);
    }
}
