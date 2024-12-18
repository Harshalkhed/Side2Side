using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private Player playerScript;

    public Material backgroundMainMat;
    public Material backgroundSubMat;

    public Material playerMainMat;
    public Material playerSubMat;

    public Material ground1Mat;
    public Material ground2Mat;

    public Material obstacleMat;

    int colorIndex;

    [System.Serializable]
    public class ColorList
    {
        public Color Ground1;
        public Color Ground2;
        public Color Obstacle;
        public Color PlayerMain;
        public Color PlayerSub;
        public Color backgroundMain;
        public Color backgroundSub;
    }

    public ColorList[] colorSets;


    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();

        // get random int to gererate color randomly
        colorIndex = Random.Range(0, colorSets.Length - 1);
        
        CallChangeColor();
    }


    public void CallChangeColor()
    {
        StartCoroutine(ChangeColor(ground1Mat, ground1Mat.color, colorSets[colorIndex].Ground1, Time.time, 1));
        StartCoroutine(ChangeColor(ground2Mat, ground2Mat.color, colorSets[colorIndex].Ground2, Time.time, 1));

        StartCoroutine(ChangeColor(obstacleMat, obstacleMat.color, colorSets[colorIndex].Obstacle, Time.time, 1));

        StartCoroutine(ChangeColor(playerMainMat, playerMainMat.color, colorSets[colorIndex].PlayerMain, Time.time, 1));
        StartCoroutine(ChangeColor(playerSubMat, playerSubMat.color, colorSets[colorIndex].PlayerSub, Time.time, 1));

        StartCoroutine(ChangeColor(backgroundMainMat, backgroundMainMat.color, colorSets[colorIndex].backgroundMain, Time.time, 1));
        StartCoroutine(ChangeColor(backgroundSubMat, backgroundSubMat.color, colorSets[colorIndex].backgroundSub, Time.time, 1));

        colorIndex = Random.Range(0, colorSets.Length - 1);
    }



    IEnumerator ChangeColor(Material mat, Color startColor, Color endColor, float time, int alpha)
    {
        float t = 0;

        while (t < 1 && !playerScript.isDead)
        {
            t += Time.deltaTime;
            mat.color = Color.Lerp(startColor, endColor, t);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(1, alpha, t));
            yield return 0;
        }

        yield break;
    }

    // Called when the player is dead.
    public IEnumerator DeadColor()
    {
        Color lastMainColor = playerMainMat.color;
        Color lastSubColor = playerSubMat.color;

        // blinks in red and black
        for (int i = 0; i < 6; i++)
        {
            playerMainMat.color = Color.red;
            playerSubMat.color = Color.red;
            yield return new WaitForSeconds(0.07f);
            playerMainMat.color = Color.black;
            playerSubMat.color = Color.black;
            yield return new WaitForSeconds(0.07f);
        }

        yield break;
    }

}
