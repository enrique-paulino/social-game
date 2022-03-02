using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float startTime = 10f;
    private float currentTime = 0f;
    public GameMaster player;
    private int tiles = 4;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (tiles != 1)
        {
            currentTime -= 1 * Time.deltaTime;
            timerText.text = currentTime.ToString("0");
        }else
        {
            //Once there's only one tile, the game is over
            timerText.text = "GAME OVER";
        }
        if (currentTime <= 0)
        {
            player.tileDestroy();
            currentTime = 10f;
            tiles = tiles - 1;
        }
    }
}
