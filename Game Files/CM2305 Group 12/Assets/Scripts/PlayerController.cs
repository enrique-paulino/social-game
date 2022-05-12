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
    const string playerWalkRight = "PlayerWalkRight";
    const string playerWalkUp = "PlayerWalkUp";
    const string playerWalkDown = "PlayerWalkDown";
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
        //Change the following conditions to change the chosen animations
        if (inputHorizontal == 0 && inputVertical == 0) 
        {
            ChangeAnimation(playerIdle);
        }
        else if (inputHorizontal > 0)
        {
            ChangeAnimation(playerWalkRight);
        }
        else if (inputHorizontal < 0)
        {
            ChangeAnimation(playerWalkLeft);
        }
        else if (inputVertical > 0)
        {
            ChangeAnimation(playerWalkUp);
        }
        else if (inputVertical < 0)
        {
            ChangeAnimation(playerWalkDown);
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
