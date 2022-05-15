using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowTile : MonoBehaviour
{
    public float timer = 5.0f;
    private float activationTime = 2.0f;
    private bool Active = false;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        timer = timer - 1 * Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        spawnTrap();
    }


    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player") && Active)
        {
            other.GetComponent<Movement>().slowDown();
        }


    }

    public void spawnTrap()
    {
        activationTime = activationTime - 1 * Time.deltaTime;
        if (activationTime <= 0)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }
        if (Active)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }


    }
}
