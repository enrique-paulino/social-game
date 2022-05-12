using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components 
    private Rigidbody2D rb;

    //Player
    float walkSpeed = 4f;
    float speedLimiter = 0.7f;
    float inputHorizontal;
    float inputVertical;

    //Animations
    Animator animator;
    string currentAnim;
    const string playerWalkLeft = "PlayerWalkLeft"; //strings are currently place holders for actual anim  names that refer to actual anims
    const string playerIdle = "PlayerIdle";



    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); //this is used any time you want to get a component from game object. gameObject with lower case g refers to the gameObject that the current script is attached to
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        ChooseAnim(inputHorizontal, inputVertical); 
    }

    void FixedUpdate()
    {
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= speedLimiter;
                inputVertical *= speedLimiter;
            }

            rb.velocity = new Vector2(inputHorizontal * walkSpeed, inputVertical * walkSpeed);
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);  
        }

    }

    //Method that changes animation

    void ChooseAnim(float inputHorizontal, float inputVertical)
    {
        
        if (inputHorizontal == 0 && inputVertical == 0)
        {
            ChangeAnimation(playerIdle);
        }
        else if (inputHorizontal > 0)
        {
            gameObject.GetComponent<Transform>().localScale = new Vector3(-0.2f, 0.3f, 0.2f);
            ChangeAnimation(playerWalkLeft);
        }
        else if (inputHorizontal < 0)
        {
            gameObject.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.3f, 0.2f);
            ChangeAnimation(playerWalkLeft);
        }
    }

    void ChangeAnimation(string newAnim)
    {
        //Stops anim from repeating 
        if (newAnim == currentAnim) return;

        animator.Play(newAnim);
        currentAnim = newAnim;
    }

}
