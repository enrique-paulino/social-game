using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gmMovement : MonoBehaviour
{

    [Header("Movement")]
    private Vector2 movement;
    private Camera cam;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    public float slowTimer;
    public bool isSlow = false;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string currentAnim;
    [SerializeField] private string[] animationNames = new string[5];

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        PlayAnimation();

    }

    private void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }

    void PlayAnimation()
    {
        if (movement == Vector2.zero)
        {
            ChangeAnimation(animationNames[0]);
            //playerModel.localScale = new Vector3(playerModel.localScale.x, 0.34f, 0.34f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //playerName.transform.localScale = Vector3.one;
            //GameObject.Find("Player Name Canvas").transform.localScale = Vector3.one;

        }
        else if (movement.x > 0)
        {
            ChangeAnimation(animationNames[1]);
            //playerModel.localScale = new Vector3(-0.34f, 0.34f, 0.34f);
            transform.localScale = new Vector3(-0.2f, transform.localScale.y, transform.localScale.z);


        }
        else if (movement.x < 0)
        {
            ChangeAnimation(animationNames[1]);
            //playerModel.localScale = new Vector3(0.34f, 0.34f, 0.34f);
            transform.localScale = new Vector3(0.2f, transform.localScale.y, transform.localScale.z);
        }
        else if (movement.y < 0)
        {
            ChangeAnimation(animationNames[1]);
            //playerModel.localScale = new Vector3(playerModel.localScale.x, 0.34f, 0.34f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (movement.y > 0)
        {
            ChangeAnimation(animationNames[1]);
            //playerModel.localScale = new Vector3(playerModel.localScale.x, 0.34f, 0.34f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }

        void ChangeAnimation(string newAnim)
        {
            if (newAnim == currentAnim) return;

            animator.Play(newAnim);
            currentAnim = newAnim;
        }

        /*if (transform.localScale.x == -1)
        {
            GameObject.Find("Player Name Canvas").transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            GameObject.Find("Player Name Canvas").transform.localScale = new Vector3(-1f, 1f, 1f);
        }*/


    }

}

