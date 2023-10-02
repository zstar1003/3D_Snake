using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public float spawnTime = 2.5f;
    public GameObject Items;
    void Start()
    {
        //InvokeRepeating("SpawnItems", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItems()
    {
        int spawnIndex = Random.Range(0, SpawnPoints.Length);
        Instantiate(Items, SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation);
    }
}
