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
    public bool isLast;
    public List<JSONEnemy> enemies;
    public List<JSONMine> mines;
}

[System.Serializable]
public class JSONEnemy
{
    public string id;
    public int spawnTime;
    public int spawnQuadrant;

    public EPoolObjectType GetId()
    {
        EPoolObjectType ret;

        switch (id)
        {
            case "enemy_default":
                ret = EPoolObjectType.enemy_default;
                break;
            case "enemy_slow":
                ret = EPoolObjectType.enemy_slow;
                break;
            case "enemy_fast":
                ret = EPoolObjectType.enemy_fast;
                break;
            case "enemy_elite":
                ret = EPoolObjectType.enemy_elite;
                break;
            default:
                ret = EPoolObjectType.enemy_default;
                break;
        }

        return ret;
    }
}

[System.Serializable]
public class JSONMine
{
    public int spawnTime;
    public int spawnQuadrant;
}