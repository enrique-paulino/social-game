using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCamera : MonoBehaviour
{
    public void ConfigureCamera(int width, int height, float mapHeight)
    {
        //Setting the cameras position to be in the middle of the tiles
        float x = width / 2;
        float y = (height / 2) - 0.25f;
        float z = gameObject.transform.position.z;
        gameObject.transform.position = new Vector3(x, y, z);

        //Setting the cameras size so it is slightly bigger than the maps height
        float size = (mapHeight / 2) + 0.5f;
        gameObject.GetComponent<Camera>().orthographicSize = size;


    }
}
