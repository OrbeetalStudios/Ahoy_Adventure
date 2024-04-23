using MEC;
using System.Collections.Generic;
using UnityEngine;

public class AssaultArea : MonoBehaviour
{
    [SerializeField] GameObject enemyObj;
    [SerializeField] private int Countdown = 5;
    [SerializeField] private int counterPlunderSfxIndex;
    [SerializeField] private int winCounterPlunderSfxIndex;
    private int resetCount;
    private bool playerInside = false;
    private bool startCount = false;
    private Enemy enemyScript;
    Material m_material;
    GameObject effect;

    private void Awake()
    {
        m_material = GetComponent<Renderer>().material;
    }
    private void Start()
    {
        resetCount = Countdown;
        enemyScript=enemyObj.GetComponent<Enemy>();   
    }
    private void OnEnable()
    {
        m_material.color = Color.red;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyObj.GetComponent<Enemy>().isEngaged = true;
            m_material.color = Color.green;
            playerInside = true;
            enemyScript.StopCoroutine("Plunder");
            effect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.engage_combat);
            PlayVFX(enemyObj, effect);
            PlaySFX(counterPlunderSfxIndex);
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
            StopVFX(effect);
            enemyObj.GetComponent<Enemy>().isEngaged = false;
            m_material.color = Color.red;
            playerInside = false;
            enemyScript.RestartPlunder();
            ResetCount();
            StopSFX(counterPlunderSfxIndex);
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
                PlaySFX(winCounterPlunderSfxIndex);
                enemyObj.GetComponent<Enemy>().isEngaged = false;
            }
            Countdown--;
            yield return Timing.WaitForSeconds(1f);
        }
        startCount=false;
    }
    private void OnDisable()
    {
        enemyObj.GetComponent<Enemy>().isEngaged = false;
        StopVFX(effect);
        StopSFX(counterPlunderSfxIndex);
        playerInside = false;
        startCount = false;
        StopCoroutine("CountDownCoroutine");
        ResetCount();
    }

    private void PlayVFX(GameObject parent, GameObject effect){
        effect.transform.position = parent.transform.position;
        effect.SetActive(true);
    }

    private void StopVFX(GameObject effect){
        effect.SetActive(false);
    }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }

    private void StopSFX(int index){
        AudioManager.Instance.StopSpecificOneShot(index);
    }
}