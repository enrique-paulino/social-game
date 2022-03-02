using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public GameObject tile; //last tile selected
    public bool control; //debug to control different chatacters
    public bool leftClick;
    public List<GameObject> allTiles;
    public PlayerExample[] allPlayers;
    // Update is called once per frame
    private void Start()
    {
        allPlayers = FindObjectsOfType<PlayerExample>();
        allTiles = GameObject.FindGameObjectsWithTag("Tile").ToList();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //gives -1 or 1 depending on lefft or right
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButton(0))
        {
            leftClick = true;
        }
    }

    private void FixedUpdate()
    {
        if (control)
        {
          rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
    //Trigger methods select the tile to be destroyed
    private void OnTriggerStay2D (Collider2D other)
    {
        if (other.gameObject.layer == 0 && leftClick)
        {
            tile = other.gameObject;
        }
        leftClick = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.layer == 0 && leftClick)
        {
            tile = other.gameObject;
        }
        leftClick = false;
    }

    public void tileDestroy()
    {
        //if gamemaster chose a tile
        if (tile != null)
        {
            foreach (PlayerExample example in allPlayers)
            {
                Destroy(tile);
                example.death(tile);
                allTiles.Remove(tile);
            }
        }
        //if they didnt, chose a random one
        else
        {
            System.Random r = new System.Random();
            int rInt = r.Next(0,allTiles.Count-1);
            foreach (PlayerExample example in allPlayers)
            {
                Destroy(allTiles[rInt]);
                example.death(allTiles[rInt]);
                allTiles.RemoveAt(rInt);
            }

        }

    }
}
