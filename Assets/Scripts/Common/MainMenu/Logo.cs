using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public Animator sprite;
    public Animator mesh;
    public GameObject plane;
    public GameObject spriteObj;
    // Start is called before the first frame update
    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ActiveFlag()
    {
        spriteObj.SetActive(false);
        plane.SetActive(true);
    }

    public void DeactiveFlag()
    {
        plane.SetActive(false);
    }
}

