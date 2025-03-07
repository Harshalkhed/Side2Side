using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{

    public GameObject[] shields;
    int currentStage;


    void Start()
    {
        currentStage = 0;
    }

    void Update()
    {
        // when stage changed
        if (currentStage != GamePlayManager.CURRENT_STAGE)
        {
            currentStage = GamePlayManager.CURRENT_STAGE;

            foreach (GameObject go in shields)
            {
                if (go != null)
                {   
                    if (go.name == "Shield " + currentStage.ToString())
                    {
                        Instantiate(go, Vector3.zero, Quaternion.identity);
                        break;
                    }
                }

            }

        }
    }


}
