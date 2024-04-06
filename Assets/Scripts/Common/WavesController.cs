using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;
using MEC;

public class WavesController : MonoBehaviour
{
    [SerializeField] private GameObject spawnPointPrefab;
    [SerializeField] private string filenameJson;
    private JSONWaves wavesDataJson = new();

    // Start is called before the first frame update
    void Start()
    {
        LoadJSON(filenameJson);
        Timing.RunCoroutine(SpawnEnemy().CancelWith(gameObject));
    }

    [ProButton]
    public void LoadJSON(string fileName)
    {
        wavesDataJson = JSONWaves.CreateFromJSON(TextFileReader.ReadFileAsText(fileName));
    }
    private IEnumerator<float> SpawnEnemy()
    {
        while (true)
        {
            GameObject enemyShip = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.enemy_default);
            enemyShip.SetActive(true);

            yield return Timing.WaitForSeconds(8f);
        }
    }
    [ProButton]
    public void SetNewSpawnPoint(float radius, float angle)
    {
        if (spawnPointPrefab == null) return;

        SpawnPoint newSpawnPoint = Instantiate(spawnPointPrefab, null).GetComponent<SpawnPoint>();
        if (newSpawnPoint)
        {
            newSpawnPoint.SetPosition(radius, angle);
        }
    }
}
