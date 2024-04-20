using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }
}
