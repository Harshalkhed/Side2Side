using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Advertisements;


public class GamePlayManager : MonoBehaviour
{

    public static int CURRENT_STAGE = 0;
    public static int currentGamePlayed = 0;

    public int eachCountForSideTouch;
    [HideInInspector]
    public int currentPercentage;
    public GameObject StageParent;
    public GameObject stageDisplayParent;
    public TextMeshProUGUI pointText;
    public Slider stageGraph;
    public Image NextStageRectangle;
    public TextMeshProUGUI CurrentStageNumber;
    public TextMeshProUGUI NextStageNumber;
    public Animator stageNumberAnimator;
    [HideInInspector]
    public int CurrentScore = 0;
    private int bestScore;
    private int targetScore = 0;
    [HideInInspector]
    public float percent;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI bestScoreAndLevel;

    public GameObject GameOverPanel;
    public GameObject tabToStartText;
    public TitleAnimatorController titleAnimatorController;
    public TextMeshProUGUI waitTimeText;


    [HideInInspector]
    public bool isStarted = false;

    private int sideTouchCount = 0;
    private int sideTouchGoal = 12;


    [HideInInspector]
    public bool isRewardAdLOADED = false;

    public TextMeshProUGUI restartOrContinueText;
    public Toggle soundOfOffToggle;


    void Awake()
    {
        Time.timeScale = 1.0f;
        restartOrContinueText.text = "RESTART";

        // Get stage number
        // If you want to start from STAGE 0, uncomment next line. 
        // PlayerPrefs.SetInt("CURRENT_STAGE", 0);
        CURRENT_STAGE = PlayerPrefs.GetInt("CURRENT_STAGE", 0);

        if (PlayerPrefs.GetString("soundOnOff", "false") == "true") soundOfOffToggle.isOn = false;
        else soundOfOffToggle.isOn = true;
    }


    void Start()
    {
        InitScore();
    }


    void Update()
    {
        GetFirstInput();
        ScoreAnimation();
    }


    void InitScore()
    {
        bestScore = PlayerPrefs.GetInt("BEST", 0);
        currentPercentage = 0;
        ResetSlider();
    }


    void GetFirstInput()
    {
        if (!IsPointerOverUIObject() && Input.GetMouseButton(0) && isStarted == false)
        {
            tabToStartText.SetActive(false);
            titleAnimatorController.CallTitleDisappearAnim();
            isStarted = true;
            StartMakingObstacle();
        }
    }

    // score text change effect
    void ScoreAnimation()
    {
        if (CurrentScore < targetScore)
        {
            CurrentScore += 1;
            pointText.text = CurrentScore.ToString();
        }
    }

    // When the player touches the side
    public void SideTouch()
    {
        AddScore(CURRENT_STAGE);
        UpdateGraph();
    }


    void UpdateGraph()
    {
        sideTouchCount += eachCountForSideTouch;
        stageGraph.value = sideTouchCount / (float)sideTouchGoal;
        percent = (sideTouchCount / (float)sideTouchGoal) * 100;

        if (sideTouchCount >= sideTouchGoal)
        {
            // if (CURRENT_STAGE % 5 == 0) ShowADs();
            StartCoroutine(StageCompleteEffect());
            sideTouchCount = 0;
            percent = 0;
        }
    }


    IEnumerator StageCompleteEffect()
    {
        GameObject.Find("Player").GetComponent<Player>().NewStageStarted();

        AddStageNumber();

        // Blink stage number UI
        for (int i = 0; i < 5; i++)
        {
            ChangeNextStageColor(Color.white, Color.black);
            yield return new WaitForSecondsRealtime(0.08f);
            ChangeNextStageColor(Color.black, Color.white);
            yield return new WaitForSecondsRealtime(0.08f);
        }
        ChangeNextStageColor(Color.white, Color.black);

        yield return new WaitForSecondsRealtime(0.5f);
        GameObject.Find("GameManager").GetComponent<ColorManager>().CallChangeColor();

        StartCoroutine(StageFadeOut());

        yield break;
    }

    void ShowADs()
    {
       // if (Advertisement.IsReady("rewardedVideo")) Advertisement.Show("rewardedVideo");
    }

    void AddScore(int n)
    {
        targetScore += n;
    }


    public IEnumerator StageFadeOut()
    {
        // Stage UI Fade Out
        float alpha = 1;
        while (alpha > 0.0f)
        {
            alpha -= 0.03f;
            StageParent.GetComponent<CanvasGroup>().alpha = alpha;
            yield return 0;
        }

        // new Stage UI appears from top
        stageNumberAnimator.Play("StageManager", -1, 0);
        StageParent.GetComponent<CanvasGroup>().alpha = 1;

        ResetSlider();
        stageGraph.value = sideTouchCount / (float)sideTouchGoal;

        yield break;
    }


    void ChangeNextStageColor(Color rectangleColor, Color textColor)
    {
        NextStageRectangle.color = rectangleColor;
        NextStageNumber.color = textColor;
    }


    void AddStageNumber()
    {
        CURRENT_STAGE++;
        PlayerPrefs.SetInt("CURRENT_STAGE", CURRENT_STAGE);
    }


    void ResetSlider()
    {
        ChangeNextStageColor(Color.black, Color.white);

        CurrentStageNumber.text = CURRENT_STAGE.ToString();
        if (CURRENT_STAGE != 55)
        {
            NextStageNumber.text = (CURRENT_STAGE + 1).ToString();
        }
        else
        {
            NextStageNumber.text = "0";
        }
    }


    public void DeleteStageUI()
    {
        stageDisplayParent.SetActive(false);
    }


    void StartMakingObstacle()
    {
        StartCoroutine(GameObject.Find("Obstacles Manager").GetComponent<ObstaclesManager>().CallMakeObstacle());
    }


    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }


    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        OpenGameOverPanel();
        Time.timeScale = 0f;
        yield break;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void OpenGameOverPanel()
    {
        DeleteStageUI();
        GameOverPanel.SetActive(true);


        finalScore.text = CurrentScore.ToString();
        if (CurrentScore > bestScore)
        {
            bestScore = CurrentScore;
            PlayerPrefs.SetInt("BEST", bestScore);
        }
        bestScoreAndLevel.text = "BEST " + bestScore + " / " + "LEVEL " + GamePlayManager.CURRENT_STAGE;
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    public void soundOnOff()
    {
        if (soundOfOffToggle.isOn == true) PlayerPrefs.SetString("soundOnOff", "false");
        if (soundOfOffToggle.isOn == false) PlayerPrefs.SetString("soundOnOff", "true");
    }









}
