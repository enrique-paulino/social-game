using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    //instance variables
    [SerializeField]
    int rowIndex;
    [SerializeField]
    int columnIndex;
    bool visited;
    [SerializeField]
    bool inPath;

    GameObject TipToeMain;

    public void SetTile(int row, int column)
    {
        this.rowIndex = row;
        this.columnIndex = column;
        this.visited = false;
        this.inPath = false;
    }

    public void Visited()
    {
        this.visited = true;
    }

    public bool IsVisited()
    {
        return this.visited;
    }

    public int GetRowIndex()
    {
        return this.rowIndex;
    }

    public int GetColumnIndex()
    {
        return this.columnIndex;
    }


    public bool CheckInPath()
    {
        return this.inPath;
    }

    public void AddToPath()
    {
        this.inPath = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.GetType() == typeof(BoxCollider2D))
        {
            if (inPath)
                {
                StartCoroutine(CorrectTile());
            }
            else
                {
                
                StartCoroutine(IncorrectTile(collision));
            }
        }
        
    }

    private System.Collections.IEnumerator CorrectTile()
    {
        SpriteRenderer tileSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        tileSpriteRenderer.color = Color.green;
        yield return new WaitForSeconds(2);
        tileSpriteRenderer.color = Color.white;
    }

    private System.Collections.IEnumerator IncorrectTile(Collider2D collision)
    {
        //Makes player transparent
        SpriteRenderer playerSpriteRenderer = collision.GetComponentInParent<SpriteRenderer>();
        playerSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0f);
        //Freezes players position so they can't move anymore
        Rigidbody2D playerRB = collision.GetComponentInParent<Rigidbody2D>();
        playerRB.constraints = RigidbodyConstraints2D.FreezePosition;
        //Changes Tile colour red
        SpriteRenderer tileSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        tileSpriteRenderer.color = Color.red;

        //Waits for a certain amount of time
        yield return new WaitForSeconds(2);

        //Changes tile colour back to white, makes the player opaque again, makes player able to move again
        tileSpriteRenderer.color = Color.white;
        //call method over here that moves the player to a start tile position
        TipToeMain = GameObject.FindGameObjectWithTag("TipToeMain");
        FloorTiles floorTiles = TipToeMain.GetComponent<FloorTiles>();
        collision.GetComponentInParent<Transform>().position = floorTiles.SetPlayerPosition();
        playerSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1f);
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;


    }
}
