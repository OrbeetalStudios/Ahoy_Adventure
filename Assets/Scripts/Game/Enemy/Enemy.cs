using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Enemy : EnemyMovement
{
    [SerializeField, Range(0f, 1f)] private float spawnChance;
    [SerializeField] private GameObject assaultArea;
    [SerializeField] private GameObject plunderBar;
    [SerializeField] private Renderer render;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private int enemyHitSfxIndex;
    [SerializeField] private int enemyStartPlunderingSfxIndex;
    [SerializeField] private int enemyCollisionSfxIndex;
    [SerializeField, Range(0, 6)] private int plunderQuantity;
    public int plunderTime;
    public int plunderDefault;
    public bool isEngaged = false;
    private Island IslandScript;

    private void Awake()
    {
        plunderDefault = plunderTime;
        GameObject islandObject = GameObject.FindWithTag("Island");
        IslandScript= islandObject.GetComponent<Island>();    
        plunderBar.GetComponent<PlunderBar>().SetMaxPlunderTime(plunderTime);
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                gameObject.SetActive(false);
                GameObject collisionEffect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.collision_with_barrels);
                PlayVFX(gameObject, collisionEffect);
                PlaySFX(enemyCollisionSfxIndex);
                break;
            case "Bullet":
                GameController.Instance.UpdateScore();
                SpawnBox();
                // TODO: if elite or not
                GameObject destroyVfx = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.enemy_destroy_vfx);
                PlayVFX(gameObject, destroyVfx);
                PlaySFX(enemyHitSfxIndex);
                //
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
        Timing.KillCoroutines("plunder_coroutine_tag"+gameObject.GetInstanceID().ToString());
        assaultArea.SetActive(false);
        plunderTime = plunderDefault;
        render.material = originalMaterial;
        plunderBar.SetActive(false);
    }
    private void StartPlunder()
    {
        plunderBar.SetActive(true);
        Timing.KillCoroutines(moveToTargetHandle);

        // rotate it by 90 degrees
        Vector3 relativePos = transform.position - Vector3.zero;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        rotation *= Quaternion.Euler(0, 90, 0);
        transform.rotation = rotation;

        // start plunder
        assaultArea.SetActive(true);
        Timing.RunCoroutine(Plunder(), "plunder_coroutine_tag"+gameObject.GetInstanceID().ToString());

        PlaySFX(enemyStartPlunderingSfxIndex);
    }
    protected IEnumerator<float> Plunder()
    {
    while (!isEngaged)
        {
            plunderBar.GetComponent<PlunderBar>().SetPlunderTime(plunderDefault-plunderTime);
            if (plunderTime <= 0)
            {
                assaultArea.SetActive(false);
                Vector3 relativePos = transform.position - Vector3.zero;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                rotation *= Quaternion.Euler(0, 180, 0);
                transform.rotation = rotation;
                IslandScript.DecreaseTreasure(plunderQuantity);
                Timing.RunCoroutine(ReturnOutsideMap(relativePos).CancelWith(gameObject));
                plunderBar.SetActive(false);
                break;
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
        if (spawnChance == 0f || Random.value > spawnChance) return; // if random value is between 0 and spawnChanche, go on and spawn a box

        GameObject box = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.box);
        box.transform.localPosition = this.transform.localPosition;
        box.SetActive(true);
    }

    private void PlayVFX(GameObject parent, GameObject effect){
        effect.transform.position = parent.transform.position;
        effect.SetActive(true);
    }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}