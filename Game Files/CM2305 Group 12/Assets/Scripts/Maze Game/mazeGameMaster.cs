using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mazeGameMaster : MonoBehaviour
{
    public GameObject[] allTiles;
    private float horizontalInput;
    private float verticalInput;
    [SerializeField]
    private float playerSpeed;
    //public bool slowCooldown, fireCooldown, holeCooldown;
    [SerializeField] private int trapTokens;
    [SerializeField] Text tokenText; 
    [SerializeField] private int slowCost= 20, fireCost = 50, holeCost= 100;
    [SerializeField] private float tokenTimer = 0, updateTimer = 1;
    public float activationTime = 2.0f;

    [SerializeField] private LayerMask trapPlaced, groundLayer;
    [SerializeField] private GameObject slowTrap, fireTrap, checkpointTile;
    [SerializeField]
    private KeyCode placefireKey = KeyCode.G, placeSlowKey = KeyCode.F, placeHoleKey = KeyCode.R;
    public Vector3 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
        int tile = Random.Range(0, allTiles.Length);
        Vector3 checkpointPostion = new Vector3 (allTiles[tile].transform.position.x, allTiles[tile].transform.position.y, allTiles[tile].transform.position.z - 2);
        GameObject.Instantiate(checkpointTile, checkpointPostion, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        /*horizontalInput = Input.GetAxis("Horizontal");
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerSpeed);
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * verticalInput * Time.deltaTime * playerSpeed);*/
        placeTrap();
        tokensManager();
    }

    private void tokensManager()
    {
        tokenTimer += Time.deltaTime;

        if (tokenTimer >= updateTimer)
        {
            tokenTimer = 0f;
            trapTokens += 15;
            tokenText.text = "Tokens: " + trapTokens.ToString();
        }
    }
    private void purchaseTrap(int cost)
    {
        trapTokens -= cost;
    }  

    private void placeTrap()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D playerCollider = gameObject.GetComponent<Collider2D>();
        Collider2D trap = Physics2D.OverlapPoint(mousePosition, trapPlaced);
        Collider2D ground = Physics2D.OverlapPoint(mousePosition, groundLayer);

        if (Input.GetKey(placeSlowKey))
        {

            if (trap == null && ground.tag == "Tile" && slowCost<= trapTokens)
            {
                Debug.Log("Place Trap");
                Instantiate(slowTrap, new Vector3(ground.transform.position.x, ground.transform.position.y, -1), ground.transform.rotation);                 
                ground.GetComponent<Renderer>();
                purchaseTrap(slowCost);
                //ground.GetComponent<normalTile>().activateTile();
                
                Debug.Log(Input.mousePosition);
            }
            else
            {
                Debug.Log("Trap here");

            }
        }
        if (Input.GetKey(placefireKey))
        {
            if (trap == null && ground.tag == "Tile" && fireCost <= trapTokens)
            {
                Debug.Log("Place Trap");
                Instantiate(fireTrap, new Vector3(ground.transform.position.x, ground.transform.position.y, -1), ground.transform.rotation);
                purchaseTrap(fireCost);
            }
            else
            {
                Debug.Log("Trap here");
            }

        }
        if (Input.GetKey(placeHoleKey))
        {
            if (trap == null && ground.tag == "Tile" && holeCost <= trapTokens)
            {
                ground.GetComponent<normalTile>().createHole();

                /*Debug.Log("Place Trap");
                Destroy(ground.gameObject);*/
                purchaseTrap(holeCost);
            }
            else
            {
                Debug.Log("Trap here");
            }
        }
    }



}
