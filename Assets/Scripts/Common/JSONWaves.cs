using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONWaves
{
    public List<JSONWave> waves;
    public static JSONWaves CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<JSONWaves>(jsonString);
    }
}

[System.Serializable]
public class JSONWave
{
    public int id;
    public List<JSONEnemy> enemies;
}

[System.Serializable]
public class JSONEnemy
{
    public string id;
    public int spawnTime;
}