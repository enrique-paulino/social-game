using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slider : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float health;
    [SerializeField] private float offset;

    private void Start()
    {
        health = player.GetComponent<mazePlayerController>().getHealth();
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offset, 0);
       
    }
}
