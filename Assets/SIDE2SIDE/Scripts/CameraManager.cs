using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject PlayerObj;
    public float smoothTime = 0.3F;
    public float yOffset;
    public float zOffset;

    private Vector3 velocity = Vector3.zero;
    private float posX = 0;

    void Update()
    {
        FollowPlayer();
    }


    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(posX, yOffset, PlayerObj.transform.position.z - zOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    // Called when the player is dead.
    public void ZoomIn()
    {
        smoothTime = 0.1f;
        posX = PlayerObj.transform.position.x;
        zOffset = 2;
        yOffset = 10;
    }
    
}
