using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExample : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 movement;
    public GameObject tile;
    public bool control;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      movement.x = Input.GetAxisRaw("Horizontal"); //gives -1 or 1 depending on lefft or right
      movement.y = Input.GetAxisRaw("Vertical");
    }

    public void FixedUpdate()
    {
      if (control)
      {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
      }
    }

    //saves the list tile the
    void OnTriggerStay2D (Collider2D other)
    {
        tile = other.gameObject;
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        tile = other.gameObject;
    }
    public void death(GameObject destroyedTile)
    {
        if(tile == destroyedTile){
            Destroy(this.gameObject);
        }
    }
}
