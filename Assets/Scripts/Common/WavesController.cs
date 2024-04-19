using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;
using MEC;
using System.Linq;
using TMPro;

public class WavesController : MonoBehaviour
{
    // Riferimenti agli oggetti UI
    [SerializeField] private TMP_Text currentWaveText;
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
    private int currentWave = 0;
    private int waveCounterUI = 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadSpawnPoints();
        LoadWavesData(filenameJson);
        Timing.RunCoroutine(SpawnEnemies().CancelWith(gameObject));
        Timing.RunCoroutine(SpawnMines().CancelWith(gameObject));
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
        int currentEnemy = -1;
        List<GameObject> activeObj = new();

        while (currentWave < numberOfWaves)
        {
            currentEnemy++;

            if (currentEnemy >= wavesDataJson.waves[currentWave].enemies.Count)
            {
                bool goOn = true;
                foreach (GameObject obj in activeObj)
                {
                    goOn &= !obj.activeSelf; 
                }
                if (goOn)
                {
                    activeObj.Clear();
                    if (!wavesDataJson.waves[currentWave].isLast) currentWave++;
                    else if (wavesDataJson.waves[currentWave].enemies.Count == 0) break;
                    currentEnemy = -1;
                    waveCounterUI++;
                }
                
                yield return Timing.WaitForSeconds(1.5f);
                continue;
            }

            JSONEnemy enemy = wavesDataJson.waves[currentWave].enemies[currentEnemy];
            GameObject enemyShip = PoolController.Instance.GetObjectFromCollection(enemy.GetId());
            if (enemyShip != null)
            {
                SetUpNewGameObject(enemyShip, (EQuadrant)enemy.spawnQuadrant);

                yield return Timing.WaitForSeconds(enemy.spawnTime);
                enemyShip.SetActive(true);
                activeObj.Add(enemyShip);
            }

            UpdateUI();
        }
    }
    private IEnumerator<float> SpawnMines()
    {
        int currentMine = -1;
        List<GameObject> activeObj = new();

        while (currentWave < numberOfWaves)
        {
            currentMine++;

            if (currentMine >= wavesDataJson.waves[currentWave].mines.Count)
            {
                bool goOn = true;
                foreach (GameObject obj in activeObj)
                {
                    goOn &= !obj.activeSelf;
                }
                if (goOn)
                {
                    activeObj.Clear();
                    if (!wavesDataJson.waves[currentWave].isLast) currentWave++;
                    else if (wavesDataJson.waves[currentWave].enemies.Count == 0) break;
                    currentMine = -1;
                }

                yield return Timing.WaitForSeconds(1.5f);
                continue;
            }

            JSONMine mine = wavesDataJson.waves[currentWave].mines[currentMine];


            GameObject newMine = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.mine);
            if (newMine != null)
            {
                SetUpNewGameObject(newMine, (EQuadrant)mine.spawnQuadrant);

                yield return Timing.WaitForSeconds(mine.spawnTime);
                newMine.SetActive(true);
            }
        }
    }
    private void SetUpNewGameObject(GameObject go, EQuadrant quadrant)
    {
        EQuadrant currQuadrant = quadrant;

        if (!System.Enum.IsDefined(typeof(EQuadrant), currQuadrant))
        {
            currQuadrant = EQuadrant.Quadrant_0_90; // default
        }

        if (spawnPoints[currQuadrant].Count != 0)
        {
            int[] newValues = Enumerable.Range(0, spawnPoints[currQuadrant].Count).Where(x => x != spawnPoints[currQuadrant].LastInd).ToArray();
            int newIndex = newValues[Random.Range(0, newValues.Length)];

            go.transform.position = spawnPoints[currQuadrant].GetAt(newIndex).transform.position;
        }
    }
    private void UpdateUI()
    {
        currentWaveText.text = "Wave: " + (waveCounterUI + 1).ToString();
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