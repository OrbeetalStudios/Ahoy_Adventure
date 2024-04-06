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
        Timing.RunCoroutine(SpawnEnemies());
    }
    [ProButton]
    public void LoadJSON(string fileName)
    {
        wavesDataJson = JSONWaves.CreateFromJSON(TextFileReader.ReadFileAsText(fileName));
    }
    private IEnumerator<float> SpawnEnemies()
    {
        int numberOfWaves = wavesDataJson.waves.Count;
        int currentWave = 0;
        int currentEnemy = -1;

        while (currentWave < numberOfWaves)
        {
            currentEnemy++;

            if (currentEnemy >= wavesDataJson.waves[currentWave].enemies.Count)
            {
                if (!wavesDataJson.waves[currentWave].isLast) currentWave++;
                currentEnemy = -1;
                continue;
            }

            GameObject enemyShip = PoolController.Instance.GetObjectFromCollection(wavesDataJson.waves[currentWave].enemies[currentEnemy].GetId());
            if (enemyShip != null)
            {
                enemyShip.SetActive(true);
            }

            yield return Timing.WaitForSeconds(wavesDataJson.waves[currentWave].enemies[currentEnemy].spawnTime);
        }
    }
    [ProButton]
    public void SetNewSpawnPoint(float radius, float angle)
    {
        if (spawnPointPrefab == null) return;

        SpawnPoint newSpawnPoint = GameObject.Instantiate(spawnPointPrefab, gameObject.transform).GetComponent<SpawnPoint>();
        if (newSpawnPoint)
        {
            newSpawnPoint.SetPosition(radius, angle);
        }
    }
}
