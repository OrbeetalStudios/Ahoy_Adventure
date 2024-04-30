using System.Collections.Generic;
using UnityEngine;

public enum EJsonTypes
{ 
    enemy,
    mine
}

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
    public float timeUntilNextWave;
    public bool isLast;
    public float speedMultiplier;

    // Json Objects, all inherit from JSONBase
    public List<JSONEnemy> enemies;
    public List<JSONMine> mines;

    public JSONBase GetObj(EJsonTypes type, int ind)
    {
        JSONBase ret;
        switch (type)
        {
            case EJsonTypes.enemy:
                ret = enemies[ind];
                break;
            case EJsonTypes.mine:
                ret = mines[ind];
                break;
            default:
                ret = enemies[ind];
                break;
        }

        return ret;
    }
    public int GetCount(EJsonTypes type)
    {
        int ret;
        switch (type)
        {
            case EJsonTypes.enemy:
                ret = enemies.Count;
                break;
            case EJsonTypes.mine:
                ret = mines.Count;
                break;
            default:
                ret = enemies.Count;
                break;
        }

        return ret;
    }
}

[System.Serializable]
public class JSONEnemy : JSONBase
{
    public string id;

    public override EPoolObjectType GetId()
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
public class JSONMine : JSONBase
{
    public override EPoolObjectType GetId()
    {
        return EPoolObjectType.mine;
    }
}

[System.Serializable]
public class JSONBase
{
    public float spawnTime;
    public int spawnQuadrant;
    public virtual EPoolObjectType GetId()
    {
        return EPoolObjectType.enemy_default;
    }
}