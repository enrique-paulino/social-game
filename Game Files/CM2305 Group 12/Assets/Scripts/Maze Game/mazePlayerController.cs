using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mazePlayerController : MonoBehaviour
{
    public int lives = 3;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float curHealth = 100;

    [SerializeField] private bool onFire;
    [SerializeField] private float fireTimer;

    public bool isGrounded = false;
    public LayerMask trapPlaced;
    public LayerMask groundLayer;


    public Slider healthBar;
    public KeyCode placefireKey = KeyCode.G;
    public KeyCode placeSlowKey = KeyCode.F;
    public KeyCode placeHoleKey = KeyCode.R;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per fra
    void Update()
    {
        fireEffect();
        isOnGround();
        healthManager();

    }



    public void fireDamage()
    {
        onFire = true;
        fireTimer = 5.0f;
        
    }

    public void fireEffect()
    {
        if (fireTimer <= 0)
        {
            onFire = false;
        }
        if (onFire == true)
        {
            fireTimer = fireTimer - 1 * Time.deltaTime;
            takeDamage(5);            
        }

    }



    private void isOnGround() 
    {
        Collider2D ground = Physics2D.OverlapPoint(transform.position, groundLayer);
        if (ground != null)
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else 
        {
            isGrounded = false;
            Debug.Log("Not grounded");
            curHealth = 0;
        }
    }

    public void takeDamage(float damAmount)
    {
        curHealth -= damAmount * Time.deltaTime;
        healthManager();
    }

    public float getHealth()
    {
        return curHealth;
    }
    public void healthManager()
    {
        healthBar.value = curHealth/maxHealth;
        if (curHealth <= 0)
        {
            die();
        }
    } 

    public void die()
    {
        lives -= 1;
        Destroy(gameObject);
        healthBar.gameObject.SetActive(false);
    }
}





