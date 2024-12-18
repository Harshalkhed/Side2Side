using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{

    public GameObject[] Obs;
    GameObject PlayerObj;
	float timeToNextObs_Min;
	float timeToNextObs_Max;


    void Start()
    {	
        PlayerObj = GameObject.Find("Player");
        CallMakeObstacle();
    }


    public IEnumerator CallMakeObstacle()
    {
        while (true)
        {
            MakeObstacle();
            yield return new WaitForSeconds(Random.Range(timeToNextObs_Min, timeToNextObs_Max));
        }
    }


    void MakeObstacle()
    {      
        if(GamePlayManager.CURRENT_STAGE >= Obs.Length){
            GamePlayManager.CURRENT_STAGE = 0;
        }
		GameObject prefab = Obs[GamePlayManager.CURRENT_STAGE];

        GameObject newObsObj = Instantiate(prefab, new Vector3(0, 0, PlayerObj.transform.position.z + 20), Quaternion.identity);
        newObsObj.transform.SetParent(transform);
		timeToNextObs_Min = newObsObj.transform.GetChild(0).GetComponent<Obstacle>().timeToNextObs_Min;
		timeToNextObs_Max = newObsObj.transform.GetChild(0).GetComponent<Obstacle>().timeToNextObs_Max;
    }


}
