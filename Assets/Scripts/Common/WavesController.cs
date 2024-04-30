using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;
using MEC;
using System.Linq;
using TMPro;

public class WavesController : MonoSingleton<WavesController>
{
    // Riferimenti agli oggetti UI
    [SerializeField] private TMP_Text currentWaveText;
    [SerializeField] private int waveStartSfxIndex;
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
    private List<WaveToSpawnType> wavesToSpawnList = new();
    [SerializeField] private List<EJsonTypes> wavesTypesToSpawn;
    private JSONWaves wavesDataJson = new();
    private int numberOfWaves;
    private int currentWave = 0;
    private int waveCounterUI = 1;
    private bool allWavesEnded = false;
    public float WaveSpeedMultiplier { get { return wavesDataJson.waves[currentWave].speedMultiplier; } }

    // Start is called before the first frame update
    void Start()
    {
        LoadSpawnPoints();
        LoadWavesData(filenameJson);

        // set up the waves to spawn
        foreach (var wave in wavesTypesToSpawn)
        {
            wavesToSpawnList.Add(new WaveToSpawnType(wave, false));
        }
    }

    public void StartGame()
    {
        Timing.RunCoroutine(SpawnManager().CancelWith(gameObject));
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
    private IEnumerator<float> SpawnManager()
    {
        allWavesEnded = false;
        float secondsBetweenWaves = 1.0f;

        // start coroutine for each wave to spawn
        foreach (var wave in wavesToSpawnList)
        {
            Timing.RunCoroutine(SpawnWave(wave.objType).CancelWith(gameObject));
        }
        PlaySFX(waveStartSfxIndex);
        while (true)
        {
            if (AreAllWavesEnded() && !allWavesEnded)
            {
                if (!wavesDataJson.waves[currentWave].isLast)
                {
                    secondsBetweenWaves = wavesDataJson.waves[currentWave].timeUntilNextWave;
                    if (currentWave + 1 < wavesDataJson.waves.Count) currentWave++; // safety
                }

                yield return Timing.WaitForSeconds(secondsBetweenWaves);
                waveCounterUI++;
                allWavesEnded = true;
                PlaySFX(waveStartSfxIndex);
            }
            else if (AreAllWavesReset()) allWavesEnded = false;
            
            UpdateUI();

            yield return Timing.WaitForOneFrame;
        }
    }
    private IEnumerator<float> SpawnWave(EJsonTypes waveObjType)
    {   
        int currentObj = -1;
        List<GameObject> activeObj = new();
        int waveIndex = wavesToSpawnList.FindIndex(x => x.objType == waveObjType);
        if (waveIndex == -1) yield break; // something went wrong, exit

        while (currentWave < numberOfWaves)
        {
            currentObj++;

            if (currentObj >= wavesDataJson.waves[currentWave].GetCount(waveObjType))
            {
                bool goOn = true;
                foreach (GameObject obj in activeObj)
                {
                    goOn &= !obj.activeSelf; 
                }
                if (goOn)
                {
                    wavesToSpawnList[waveIndex].isWaveEnded = true;
                    activeObj.Clear();
                    if (allWavesEnded)
                    {
                        wavesToSpawnList[waveIndex].isWaveEnded = false;
                        
                        if (wavesDataJson.waves[currentWave].GetCount(waveObjType) == 0) break;
                        currentObj = -1;         
                    }
                }

                yield return Timing.WaitForSeconds(1);
                continue;
            }

            JSONBase jsonObjToSpawn = wavesDataJson.waves[currentWave].GetObj(waveObjType, currentObj);
            GameObject newObj = PoolController.Instance.GetObjectFromCollection(jsonObjToSpawn.GetId());
            if (newObj != null)
            {
                SetUpNewGameObject(newObj, (EQuadrant)jsonObjToSpawn.spawnQuadrant);

                yield return Timing.WaitForSeconds(jsonObjToSpawn.spawnTime);
                newObj.SetActive(true);
                activeObj.Add(newObj);
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
        currentWaveText.text = "Wave: " + waveCounterUI.ToString();
    }
    private bool AreAllWavesEnded()
    {
        return !wavesToSpawnList.Exists(x => x.isWaveEnded == false);
    }
    private bool AreAllWavesReset()
    {
        return !wavesToSpawnList.Exists(x => x.isWaveEnded == true);
    }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}

public class WaveToSpawnType
{
    public EJsonTypes objType;
    public bool isWaveEnded;

    public WaveToSpawnType(EJsonTypes a, bool b)
    {
        objType = a;
        isWaveEnded = b;
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