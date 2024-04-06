
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField, Range(0f, 180f)]//180 more rendering distance with this settings
    private float spawnRadius;
    public Vector3 resetPosition;
    private List<Vector3> spawnPoints = new List<Vector3>();
    [SerializeField]
    private EnemyMovement movement;
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.5f;

    protected void Awake()
    {
        FindSpawnPoint();
    }
    protected void OnEnable()
    {
        SetInitialPosition();
    }
    public void SetInitialPosition()
    {
        Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        transform.position = randomSpawnPoint;
    }
    public void FindSpawnPoint()
    {
        //Create Random Spawn Point
        for (int i = 0; i < 360; i++)//casual point for 360 degrees push in list!
        {
            float angle = Random.Range(0f, Mathf.PI * 2f); // Generating casual angle

            float x = spawnRadius * Mathf.Cos(angle);
            float z = spawnRadius * Mathf.Sin(angle);

            Vector3 randomPoint = new Vector3(x, 0f, z);
            spawnPoints.Add(randomPoint);
        }
    }

     void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                GameController.Instance.UpdateLife();
                gameObject.SetActive(false);
                break;
            case "Bullet":
                GameController.Instance.UpdateScore();
                SpawnBox();
                other.gameObject.SetActive(false);//Deactivate Bullet
                gameObject.SetActive(false);
                break;
            case "Island":
                movement.StartPlunder();
                break;
        }        
     }
    private void SpawnBox()
    {
        if (Random.Range(0f, 1f) > spawnChance) return; // if random value is between 0 and spawnChanche, go on and spawn a box

        GameObject box = PoolController.Instance.GetObjectFromCollection(EPoolObjectType.box);
        box.transform.localPosition = this.transform.localPosition;
        box.SetActive(true);
    }
}
