using UnityEngine;
using System.Collections.Generic;

public class FloorTiles : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject squareTile;
    public GameObject bottomLeftTile;
    public GameObject bottomMiddleTile;
    public GameObject bottomRightTile;
    public GameObject topLeftTile;
    public GameObject topMiddleTile;
    public GameObject topRightTile;
    public GameObject sideBorder;
    public int width = 6;
    public int height = 4;
    public float tileGap = 1.1f;
    public Tile[,] tilesArray;
    public setCamera mainCamera;
    private float mapHeight;// Used to determine the size that the camera should be
    

    void Start()
    {
        
        GenerateFloorTiles();
        ConstructPath();
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = SetPlayerPosition();
        mainCamera.ConfigureCamera(width, height, mapHeight);
    }

    void GenerateFloorTiles()
    {
        GenerateBorderTiles();
        tilesArray = new Tile[height, width];
        for (int row = 0; row < height; ++row) //row is equivelant to y
        {
            for (int col = 0; col < width; ++col) //col is equivelant to x
            {
                float rowPos = row * tileGap;
                float colPos = col * tileGap;
                
                GameObject tile = Instantiate(squareTile, new Vector2(colPos, rowPos), Quaternion.identity,gameObject.transform);
                Tile tileComponent = tile.GetComponent<Tile>();
                tileComponent.SetTile(row, col);
                tilesArray[row, col] = tileComponent;
                //Debug.Log(tileComponent.getRowIndex() + " " + tileComponent.getColumnIndex());

            }
        }
        foreach(Tile t in tilesArray)
        {
            Debug.Log(t.GetRowIndex() + " " + t.GetColumnIndex());
        }
    }

    void GenerateBorderTiles()
    {
        float lastRow = (height-1)*tileGap;
        float lastCol = (width - 1) * tileGap;
        Vector3 adjustedRotation = new Vector3(0, 0, -90);

        //Top bottom tiles
        for (int i = 0; i <= height; ++i)
        {
            if(i == 0)
            {
                GameObject tile = Instantiate(bottomRightTile, new Vector2(0 - tileGap, (i*tileGap) - (tileGap / 2)), Quaternion.identity, gameObject.transform);
                GameObject topTile = Instantiate(topRightTile, new Vector2(lastCol + tileGap, (i * tileGap) - (tileGap / 2)), Quaternion.identity, gameObject.transform);
                tile.transform.eulerAngles = adjustedRotation;
                topTile.transform.eulerAngles = adjustedRotation;

            }
            else if(i == height)
            {
                GameObject tile = Instantiate(bottomLeftTile, new Vector2(0 - tileGap, (i * tileGap) - (tileGap / 2)), Quaternion.identity, gameObject.transform);
                GameObject topTile = Instantiate(topLeftTile, new Vector2(lastCol + tileGap, (i * tileGap) - (tileGap / 2)), Quaternion.identity, gameObject.transform);
                tile.transform.eulerAngles = adjustedRotation;
                topTile.transform.eulerAngles = adjustedRotation;
            }
            else
            {
                GameObject tile = Instantiate(bottomMiddleTile, new Vector2(0 - tileGap, (i * tileGap) - (tileGap / 2)), Quaternion.identity, gameObject.transform);
                GameObject topTile = Instantiate(topMiddleTile, new Vector2(lastCol + tileGap, (i * tileGap) - (tileGap / 2)), Quaternion.identity, gameObject.transform);
                tile.transform.eulerAngles = adjustedRotation;
                topTile.transform.eulerAngles = adjustedRotation;
            }
        }
        //Side Tiles
        for (int j = 0; j < width; ++j)
        {
            GameObject topBorder = Instantiate(sideBorder, new Vector2((j*tileGap), (float)(lastRow + tileGap)), Quaternion.identity, gameObject.transform);
            topBorder.transform.eulerAngles = new Vector3(0, 0, 180); ;
            GameObject bottomBorder = Instantiate(sideBorder, new Vector2((j*tileGap), (float)(0 - tileGap)), Quaternion.identity, gameObject.transform);
            mapHeight = (lastRow + tileGap) - (0 - tileGap);
        }


    }

    void ConstructPath()
    {
        int[] right = new int[2] { 0, 1 };
        int[] up = new int[2] { 1, 0 };
        int[] down = new int[2] { -1, 0 };
        //int[,] directions = new int[3, 2] { { 0, 1 }, { 1, 0 }, { -1, 0 } };
        int[][] directions = new int[3][] { right, up, down };

        int start = Random.Range(0, height);
        Tile current = tilesArray[start, 0];
        current.Visited();
        current.AddToPath(); 
        while (current.GetColumnIndex() < width - 1)
        {
            List<int[]> nextDirectionsList = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                int[] direction = directions[i];
                if (IsUnvisitedTile(current.GetRowIndex()+direction[0], current.GetColumnIndex()+ direction[1]))
                {
                    nextDirectionsList.Add(direction);
                }
            }
            int[][] nextDirections = nextDirectionsList.ToArray();
            int length = nextDirections.GetLength(0);
            int rIndex = Random.Range(0, length);
            Tile tempCurrent = tilesArray[current.GetRowIndex() + nextDirections[rIndex][0], current.GetColumnIndex() + nextDirections[rIndex][1]];
            current = tempCurrent;
            current.Visited();
            current.AddToPath();
        }

    }

    //Checks if the indexes that are passed in would be in the tiles array and then it checks if the tile has been visited
    bool IsUnvisitedTile(int row, int col)
    {
        if((row >= 0 && row < height) && (col >= 0 && col < width))
        {
            if (!(tilesArray[row, col].IsVisited()))
            {
                return true;
            }
            else
            {
                return false;
            }           
        }
        else
        {
            return false;
        }
    }

    //Sets the players position to be a random position on the start column
    public Vector3 SetPlayerPosition()
    {
        int start = Random.Range(1, height-1);
        float x = (tilesArray[start, 0].transform.position.x) - tileGap;
        float y = tilesArray[start, 0].transform.position.y;
        float z = tilesArray[start, 0].transform.position.z;
        Vector3 playerPos = new Vector3(x, y, z);
        return playerPos;
    }
}
    