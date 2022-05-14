using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireScript : MonoBehaviour
{
    public float timer = 5.0f;
    private float activationTime = 2.0f;
    private bool Active = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
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
            other.GetComponent<mazePlayerController>().fireDamage();
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

