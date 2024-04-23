using MEC;
using System.Collections.Generic;
using UnityEngine;

public class AssaultArea : MonoBehaviour
{
    [SerializeField] GameObject enemyObj;
    [SerializeField] private int Countdown = 5;
    private int resetCount;
    private bool playerInside = false;
    private bool startCount = false;
    private Enemy enemyScript;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        resetCount = Countdown;
        enemyScript=enemyObj.GetComponent<Enemy>();   
    }
    private void OnEnable()
    {
        spriteRenderer.color = Color.red;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = Color.green;
            playerInside = true;
            enemyScript.StopCoroutine("Plunder");
            GameObject effect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.engage_combat);
            PlayVFX(enemyObj, effect);
            if (startCount == false)
            {
                Timing.RunCoroutine(CountDownCoroutine().CancelWith(gameObject));
            }
        }  
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = Color.red;
            playerInside = false;
            enemyScript.RestartPlunder();
            ResetCount();
        }
    }
    public void ResetCount()
    {
      Countdown=resetCount;
    }
    protected IEnumerator<float> CountDownCoroutine()
    {
        startCount = true;
        while (playerInside==true)
        {
            if (Countdown ==0)
            {
                enemyObj.SetActive(false);
                playerInside =false;   
            }
            Countdown--;
            yield return Timing.WaitForSeconds(1f);
        }
        startCount=false;
    }
    private void OnDisable()
    {
        playerInside = false;
        startCount = false;
        StopCoroutine("CountDownCoroutine");
        ResetCount();
    }

    private void PlayVFX(GameObject parent, GameObject effect){
        effect.transform.position = parent.transform.position;
        effect.SetActive(true);
    }
}