using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Enemy : EnemyMovement
{
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.5f;
    [SerializeField] private GameObject assaultArea;
    [SerializeField] private Renderer render;
    [SerializeField] private Material originalMaterial;
    public int plunderTime;
    public int plunderDefault;

    private void Awake()
    {
        plunderDefault = plunderTime;
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                GameController.Instance.UpdateLife();
                gameObject.SetActive(false);
                break;
            case "Bullet":
                GameController.Instance.UpdateScore();
                SpawnBox();
                other.gameObject.SetActive(false);//Deactivate Bullet
                gameObject.SetActive(false);
                break;
            case "Island":
                StartPlunder();
                break;
        }        
    }
    private void OnDisable()
    {
        assaultArea.SetActive(false);
        StopAllCoroutines();
        plunderTime = plunderDefault;
        render.material = originalMaterial;
    }
    private void StartPlunder()
    {
        Timing.KillCoroutines(moveToTargetHandle);

        // rotate it by 90 degrees
        Vector3 relativePos = transform.position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        rotation *= Quaternion.Euler(0, 90, 0);
        transform.rotation = rotation;

        // start plunder
        assaultArea.SetActive(true);
        Timing.RunCoroutine(Plunder());
    }
    protected IEnumerator<float> Plunder()
    {
        while (true)
        {
            if (plunderTime <= 0)
            {
                assaultArea.SetActive(false);
                Vector3 relativePos = transform.position - Vector3.zero;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation *= Quaternion.Euler(0, 180, 0);
                transform.rotation = rotation;
                //Island.Instance.DecreaseTreasure();
                Timing.RunCoroutine(ReturnOutsideMap(relativePos));
            }
            plunderTime--;
            yield return Timing.WaitForSeconds(1f);
        }
    }
    public void RestartPlunder()
    {
        Timing.RunCoroutine(Plunder().CancelWith(gameObject));
    }
    private void SpawnBox()
    {
        if (Random.Range(0f, 1f) > spawnChance) return; // if random value is between 0 and spawnChanche, go on and spawn a box

        GameObject box = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.box);
        box.transform.localPosition = this.transform.localPosition;
        box.SetActive(true);
    }
}