using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{


    GamePlayManager GamePlayManagerScript;

    float angle = 1.57f;
    float angleForUpDown = 0;
    float upDownSpeed = 10;

    public float MoveForwardSpeed = 2f;
    float MoveSideToSideCurrentSpeed = 0f;

    public float MoveSideToSideMinSpeed = 0.1f;
    public float MoveSideToSideMaxSpeed = 4f;


    public GameObject deadEffectprefab;

    public ParticleSystem accelerationEffect;

    [HideInInspector]
    public bool isDead = false;

    public GameObject beforeDeadEffectPrefab;


    public GameObject pointText;

    bool isEnterSideTriggerAlready = false;


    public AudioClip SideTouchClip;
    public AudioClip DeadBeforeClip;
    public AudioClip DeadAfterClip;
    AudioSource source;

    public MeshRenderer player1;
    public MeshRenderer player2;

    bool isAudioOn;


    [HideInInspector]
    public bool beginToCount = false;

    public UnityEngine.UI.Toggle speakerToggle;


    void Start()
    {
        source = GetComponent<AudioSource>();
        GamePlayManagerScript = GameObject.Find("GameManager").GetComponent<GamePlayManager>();

        ////Initialize time scale
        Time.timeScale = 1.0f;

        //Initialize player speed and effect
        MoveSideToSideCurrentSpeed = MoveSideToSideMinSpeed;
        accelerationEffect.Stop();
    }



    void Update()
    {
        // Floating motion of player
        MoveUpDown();

        // The player keeps moving forward
        MoveForward();

        // move left and right when screen touched.
        MoveSideToSide();
    }


    void MoveUpDown()
    {
        if (isDead) return;

        Vector3 pos = transform.position;
        pos.y += Mathf.Cos(angleForUpDown) * 0.01f;
        transform.position = pos;

        angleForUpDown += Time.deltaTime * upDownSpeed;
    }


    void MoveForward()
    {
        if (!GamePlayManagerScript.isStarted) return;
        if (isDead) return;

        Vector3 pos = transform.position;
        pos.z += Time.deltaTime * MoveForwardSpeed;
        transform.position = pos;
    }


    void MoveSideToSide()
    {
        if (isDead) return;

        if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
        {
            if (MoveSideToSideCurrentSpeed < MoveSideToSideMaxSpeed)
            {
                MoveSideToSideCurrentSpeed += 0.1f;
            }
            accelerationEffect.Play();
        }
        else
        {
            if (MoveSideToSideCurrentSpeed > MoveSideToSideMinSpeed)
            {
                MoveSideToSideCurrentSpeed -= 0.1f;
                accelerationEffect.Stop();
            }
        }

        angle += Time.deltaTime * MoveSideToSideCurrentSpeed;

        Vector3 pos = transform.position;
        pos.x = Mathf.Cos(angle) * 2.5f;
        transform.position = pos;
    }



    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall" && isDead == false)
        {
            StartCoroutine(Dead(other));
        }
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SideTrigger" && isEnterSideTriggerAlready == false)
        {
            SideTouch();
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SideTrigger")
        {
            isEnterSideTriggerAlready = false;
        }
    }



    void SideTouch()
    {
        if (GamePlayManagerScript.isStarted == false || beginToCount == false) return;

        isEnterSideTriggerAlready = true;

        DisplayPointTextEffect();

        if (PlayerPrefs.GetString("soundOnOff", "false") == "true" && SideTouchClip != null) source.PlayOneShot(SideTouchClip, 1);

        GamePlayManagerScript.SideTouch();
    }



    void DisplayPointTextEffect()
    {
        GameObject pointTextObj = Instantiate(pointText, transform.position + new Vector3(0, 0, 1f), Quaternion.identity);
        pointTextObj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "+" + GamePlayManager.CURRENT_STAGE;

        Destroy(pointTextObj, 1f);
    }


    IEnumerator Dead(Collision other)
    {
        isDead = true;
        Instantiate(beforeDeadEffectPrefab, other.contacts[0].point, Quaternion.identity);
        GameObject.Find("Main Camera").GetComponent<CameraManager>().ZoomIn();
        StartCoroutine(GameObject.Find("GameManager").GetComponent<ColorManager>().DeadColor());
        if (PlayerPrefs.GetString("soundOnOff", "false") == "true" && DeadBeforeClip != null) source.PlayOneShot(DeadBeforeClip, 1);

        yield return new WaitForSeconds(1.0f);
        if (PlayerPrefs.GetString("soundOnOff", "false") == "true" && DeadAfterClip != null) source.PlayOneShot(DeadAfterClip, 1);
        GetComponent<BoxCollider>().enabled = false;
        Instantiate(deadEffectprefab, transform.position, Quaternion.identity);
        GameObject.Find("GameManager").GetComponent<GamePlayManager>().GameOver();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        yield break;
    }



    public void NewStageStarted()
    {
        beginToCount = false;
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }




}




