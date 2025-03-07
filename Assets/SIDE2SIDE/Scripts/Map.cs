using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public GameObject Player;
	float mapLength = 20;

    void Update()
    {   
        // If the distance from map object to player object is far enough
        if (Player.transform.position.z - transform.position.z > (mapLength + mapLength/4))
        {
            // Reposition this map object
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + mapLength*3);
        }
    }
}
