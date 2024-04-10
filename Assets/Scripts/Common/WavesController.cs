using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;
using MEC;
using System.Linq;

public class WavesController : MonoBehaviour
{
    enum EQuadrant
    {
        Quadrant_0_90 = 1,
        Quadrant_90_180 = 2,
        Quadrant_180_270 = 3,
        Quadrant_270_360 = 4
    }
    [SerializeField] private GameObject spawnPointPrefab;
    private Dictionary<EQuadrant, Coll<SpawnPoint>> spawnPoints = new Dictionary<EQuadrant, Coll<SpawnPoint>>()
    {
        {EQuadrant.Quadrant_0_90, new Coll<SpawnPoint>() },
        {EQuadrant.Quadrant_90_180, new Coll<SpawnPoint>() },
        {EQuadrant.Quadrant_180_270, new Coll<SpawnPoint>() },
        {EQuadrant.Quadrant_270_360, new Coll<SpawnPoint>() }
    };
    [SerializeField] private string filenameJson;
    private JSONWaves wavesDataJson = new();
    private int numberOfWaves;

    // Start is called before the first frame update
    void Start()
    {
        LoadSpawnPoints();
        LoadWavesData(filenameJson);
        Timing.RunCoroutine(SpawnEnemies());
        Timing.RunCoroutine(SpawnMines());
    }
    private void LoadSpawnPoints()
    {
        SpawnPoint sp;
        for (int i = 0; i < transform.childCount; i++)
        {
            sp = transform.GetChild(i).gameObject.GetComponent<SpawnPoint>();
            if (sp != null)
            {
                if (sp.currAngle >= 0 && sp.currAngle <= 90) spawnPoints[EQuadrant.Quadrant_0_90].Add(sp);
                else if (sp.currAngle > 90 && sp.currAngle <= 180) spawnPoints[EQuadrant.Quadrant_90_180].Add(sp);
                else if (sp.currAngle > 180 && sp.currAngle <= 270) spawnPoints[EQuadrant.Quadrant_180_270].Add(sp);
                else spawnPoints[EQuadrant.Quadrant_270_360].Add(sp);
            }
        }
    }
    [ProButton]
    public void LoadWavesData(string fileName)
    {
        wavesDataJson = JSONWaves.CreateFromJSON(TextFileReader.ReadFileAsText(fileName));
        numberOfWaves = wavesDataJson.waves.Count;
    }
    private IEnumerator<float> SpawnEnemies()
    {
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

            JSONEnemy enemy = wavesDataJson.waves[currentWave].enemies[currentEnemy];
            GameObject enemyShip = PoolController.Instance.GetObjectFromCollection(enemy.GetId());
            if (enemyShip != null)
            {
                EQuadrant currQuadrant = (EQuadrant)enemy.spawnQuadrant;
                if (spawnPoints[currQuadrant].Count != 0)
                {
                    int[] newValues = Enumerable.Range(0, spawnPoints[currQuadrant].Count).Where(x => x != spawnPoints[currQuadrant].LastInd).ToArray();
                    int newIndex = newValues[Random.Range(0, newValues.Length)];

                    enemyShip.transform.position = spawnPoints[currQuadrant].GetAt(newIndex).transform.position;
                }

                yield return Timing.WaitForSeconds(enemy.spawnTime);
                enemyShip.SetActive(true);
            }
        }
    }
    private IEnumerator<float> SpawnMines()
    {
        int currentWave = 0;
        int currentMine = -1;

        while (currentWave < numberOfWaves)
        {
            currentMine++;

            if (currentMine >= wavesDataJson.waves[currentWave].mines.Count)
            {
                if (!wavesDataJson.waves[currentWave].isLast) currentWave++;
                currentMine = -1;
                continue;
            }

            JSONMine mine = wavesDataJson.waves[currentWave].mines[currentMine];
            GameObject newMine = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.mine);
            if (newMine != null)
            {
                EQuadrant currQuadrant = (EQuadrant)mine.spawnQuadrant;
                if (spawnPoints[currQuadrant].Count != 0)
                {
                    int[] newValues = Enumerable.Range(0, spawnPoints[currQuadrant].Count).Where(x => x != spawnPoints[currQuadrant].LastInd).ToArray();
                    int newIndex = newValues[Random.Range(0, newValues.Length)];

                    newMine.transform.position = spawnPoints[currQuadrant].GetAt(newIndex).transform.position;
                }

                yield return Timing.WaitForSeconds(mine.spawnTime);
                newMine.SetActive(true);
            }
        }
    }
}

public class Coll<T>
{
    List<T> coll = new List<T>();
    int lastInd = -1;

    public int Count { get { return coll.Count; } }
    public int LastInd { get { return lastInd; } }

    public void Add(T item)
    {
        coll.Add(item);
    }
    public T GetAt(int index)
    {
        lastInd = index;
        return coll.ElementAt(index);
    }
}