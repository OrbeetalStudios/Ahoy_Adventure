using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultArea : MonoBehaviour
{
    [SerializeField]
    GameObject enemyObj;
    [SerializeField]
    private int Countdown=5;
    private int resetCount;
    private bool playerInside=false;
    private bool startCount=false;
    private EnemyMovement enemyMovScript;

    private void OnEnable()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
    }
    private void Start()
    {
        resetCount = Countdown;
        enemyMovScript=enemyObj.GetComponent<EnemyMovement>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.green;
            playerInside = true;
            enemyMovScript.inPlunder = false;
            enemyMovScript.StopCoroutine("Plunder");
            if (startCount == false)
            {
                Timing.RunCoroutine(countDownCoroutine().CancelWith(gameObject));
            }
        }  
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            playerInside = false;
            enemyMovScript.inPlunder = true;
            enemyMovScript.RestartPlunder();
            ResetCount();
        }
    }
    public void ResetCount()
    {
      Countdown=resetCount;
    }
    protected IEnumerator<float> countDownCoroutine()
    {
        startCount = true;
        while (playerInside==true)
        {
            if (Countdown ==0)
            {
                enemyObj.SetActive(false);
                playerInside =false;   
            }
           // Debug.Log("Tempo " + Countdown);
            Countdown--;
            yield return Timing.WaitForSeconds(1f);
        }
        startCount=false;
    }
    private void OnDisable()
    {
        playerInside = false;
        startCount = false;
        StopCoroutine("countDownCoroutine");
        ResetCount();
    }
}