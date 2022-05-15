using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerFall : MonoBehaviour
{

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        int randomPicker = Random.Range(1, 5);
        if(randomPicker == 1){
            transform.position = new Vector2(-6.679774f, 4.641911f);
        }else if(randomPicker == 2){
            transform.position = new Vector2(7.679299f, 5.190889f);
        } else if(randomPicker == 3){
            transform.position = new Vector2(7.470084f, -9.245568f);
        }
        else
        {
            transform.position = new Vector2(-7.233826f, -9.670664f);
        }
    }
}
