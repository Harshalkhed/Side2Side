using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    GameObject PlayerObj;
    int currentStage;

    public int zOffset = 3;

    void Start()
    {
        currentStage = GamePlayManager.CURRENT_STAGE;
        PlayerObj = GameObject.Find("Player");
    }


    void Update()
    {
        SetPosition();
        CheckStageNumberForDestroy();
    }


    void SetPosition()
    {
        Vector3 playerPosition = PlayerObj.transform.position;
        transform.position = new Vector3(0, 0, playerPosition.z + zOffset);
    }


    void CheckStageNumberForDestroy()
    {
        if (currentStage != GamePlayManager.CURRENT_STAGE)
        {
            Destroy(this.gameObject);
        }
    }
}
