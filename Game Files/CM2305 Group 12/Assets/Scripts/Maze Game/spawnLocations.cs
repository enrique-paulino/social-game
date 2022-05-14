using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnLocations : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject player;

    void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");    
    }

    void Start()
    {
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnPlayer()
    {
        int spawn = Random.Range(0, spawnPoints.Length);
        GameObject.Instantiate(player, spawnPoints[spawn].transform.position, Quaternion.identity);
    }

}
