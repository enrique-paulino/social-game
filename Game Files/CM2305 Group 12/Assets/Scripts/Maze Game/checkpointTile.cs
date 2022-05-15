using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointTile : MonoBehaviour
{
    public GameObject[] spawnPoints;
    //private Vector3 location;
    void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SafeZone");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int spawnLocation = Random.Range(0, spawnPoints.Length);
            Vector3 location = spawnPoints[spawnLocation].transform.position;
            other.transform.position = location;
        }
    }
}
