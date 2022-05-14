using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalTile : MonoBehaviour
{
    private bool active = false;
    private float timer = 1.5f;
    private bool hole = false;
    [SerializeField] private float holeTimer;
    private Renderer tileRenderer;
    
    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        
        
        if (hole == true)
        {
            holeTimer -= 1 * Time.deltaTime;
            if (holeTimer <= 0)
            {
                Destroy(gameObject);
            }
            
        }
    }

    // Update is called once per frame
    private void OnMouseOver()
    {
        if (!active)
        {
            tileRenderer.material.color = Color.green;
        }
        else
        {
            tileRenderer.material.color = Color.white;
        }
        
    }

    private void OnMouseExit()
    {
        tileRenderer.material.color = Color.white;
    }

    public void activateTile()
    {
        active = true;
        timer = 2.0f;
    }

    public void createHole()
    {
        active = true;
        timer = 2.0f;
        hole = true;        
    }
    
}
