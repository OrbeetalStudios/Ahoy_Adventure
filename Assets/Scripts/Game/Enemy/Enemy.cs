using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Enemy : EnemyMovement
{
    private enum EEnemyType
    { 
        enemy_default = 0,
        enemy_slow = 1,
        enemy_fast = 2,
        enemy_elite = 3
    }

    [SerializeField, Range(0f, 1f)] private float spawnChanceBox;
    [SerializeField, Range(0f, 1f)] private float spawnChanceMine;
    [SerializeField] private float spawnMineThreshold = 90f;
    [SerializeField] private GameObject assaultArea;
    [SerializeField] private GameObject plunderBar;
    [SerializeField] private Renderer render;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private int enemyHitSfxIndex;
    [SerializeField] private int enemyStartPlunderingSfxIndex;
    [SerializeField] private int enemyCollisionSfxIndex;
    [SerializeField, Range(0, 3)] private float plunderQuantity;
    [SerializeField] private int MaxLives = 1;
    [SerializeField] private EEnemyType enemyType;
    private int currentLives;
    public int plunderTime;
    public int plunderDefault;
    public bool isEngaged = false;

  
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
                currentLives--;
                if (currentLives <= 0) // enemy dead
                {
                    OnDeath();
                } else {
                    GameObject eliteEnemyHitEffect = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.elite_enemy_hit);
                    PlayVFX(gameObject, eliteEnemyHitEffect);
                    PlaySFX(enemyHitSfxIndex);
                }
                // TODO: if elite or not
                other.gameObject.SetActive(false);//Deactivate Bullet
                break;
            case "Island":
                StartPlunder();
                break;
        }        
    }
    private void OnEnable()
    {
        base.OnEnable();
        currentLives = MaxLives;
        plunderDefault = plunderTime;
        plunderBar.GetComponent<PlunderBar>().SetMaxPlunderTime(plunderTime);
    }
    private void OnDisable()
    {
        Timing.KillCoroutines("plunder_coroutine_tag"+gameObject.GetInstanceID().ToString());
        assaultArea.SetActive(false);
        plunderTime = plunderDefault;
        render.material = originalMaterial;
        plunderBar.SetActive(false);
    }
    private void OnDeath()
    {
        GameObject destroyVfx = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.enemy_destroy_vfx);
        PlayVFX(gameObject, destroyVfx);
        PlaySFX(enemyHitSfxIndex);
        GameController.Instance.UpdateScore();
        SpawnBox();

        // se elite, dai un doblone
        if (enemyType == EEnemyType.enemy_elite)
        {
            PowerUpController.Instance.ActivatePowerUp(EPowerUpType.DoubloonUp);
        }

        gameObject.SetActive(false);
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
                Island.Instance.DecreaseTreasure(plunderQuantity);
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
        float[] weights = new float[]{ spawnChanceBox, spawnChanceMine, 1 - (spawnChanceBox + spawnChanceMine) };
        int index = WeightedRandom.GetRandomWeightedIndex(weights);

        if (index >= 2) return;
        if (index == 1) // mine
        {
            if (Vector3.Distance(this.transform.position, targetPosition) < spawnMineThreshold) 
                return;
        }

        GameObject box = PoolController.Instance.GetObjectFromCollection(index == 0 ? EPoolObjectType.box : EPoolObjectType.mine);
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