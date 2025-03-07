using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    GameObject PlayerObj;
    int stageNumber;
    Rigidbody rb;
    bool isStageOver = false;

    public bool randomPositionX;
    public bool randomPositionY;

    [System.Serializable]
    public class RandomAngle
    {
        public bool x;
        public bool y;
        public bool z;
    }

    public RandomAngle randomAngle;

    public float timeToNextObs_Min;
    public float timeToNextObs_Max;

    [Space]
    public float velocity_X_Min;
    public float velocity_X_Max;
    public float velocity_Y;
    public float velocity_Z_Min;
    public float velocity_Z_Max;

    [Space]
    public bool useRigidBody = true;
    public bool useGravity = true;


    [Space]
    public float Move_X_Distance = 2.5f;
    public float Move_X_Speed = 0;
    public bool Move_X_RandomAngle = true;

    [Space]
    public float Move_Y_Distance = 2.5f;
    public float Move_Y_Speed = 0;
    public bool Move_Y_RandomAngle = true;

    [Space]
    public float Move_Z_Distance = 2.5f;
    public float Move_Z_Speed = 0;
    public bool Move_Z_RandomAngle = true;

    float MoveAngle_X;
    float MoveAngle_Y;
    float MoveAngle_Z;


    [Space]
    public float RotationSpeedX_Min = 0;
    public float RotationSpeedX_Max = 0;
    public float RotationSpeedY_Min = 0;
    public float RotationSpeedY_Max = 0;
    public float RotationSpeedZ_Min = 0;
    public float RotationSpeedZ_Max = 0;
    float rotationSpeedX;
    float rotationSpeedY;
    float rotationSpeedZ;


    public float UpdownSpeed = 0;
    Vector3 startPos;
    public Vector3 addForce = new Vector3(0,0,0);

    void Start()
    {
        stageNumber = GamePlayManager.CURRENT_STAGE;

        rb = GetComponent<Rigidbody>();
        PlayerObj = GameObject.Find("Player");

        startPos = transform.position;

        InitStartAngle();
        InitStartPosition();
        InitRotationSpeed();
        InitStartAngleForMove();
        InitVelocity();

        if (!useRigidBody && rb != null) rb.isKinematic = true;
        if (!useGravity && rb != null) rb.useGravity = false;
    }



    void InitStartAngleForMove()
    {
        if (Move_X_RandomAngle == true) MoveAngle_X = Random.Range(0, 90);
        else MoveAngle_X = 0;

        if (Move_Y_RandomAngle == true) MoveAngle_Y = Random.Range(0, 90);
        else MoveAngle_Y = 0;

        if (Move_Z_RandomAngle == true) MoveAngle_Z = Random.Range(0, 90);
        else MoveAngle_Z = 0;
    }


    void InitVelocity()
    {
        float velX = Random.Range(velocity_X_Min, velocity_X_Max);
        float velZ = Random.Range(velocity_Z_Min, velocity_Z_Max);

        rb.velocity = new Vector3(velX, velocity_Y, velZ);
    }



    void Update()
    {
        if (PlayerObj == null || PlayerObj.GetComponent<Player>().isDead || isStageOver) return;

        if ((rotationSpeedX != 0 || rotationSpeedY != 0 || rotationSpeedZ != 0)) Rotation();
        if (Move_X_Speed != 0 || Move_Y_Speed != 0 || Move_Z_Speed != 0) Move();

        //Limit the obstacle speed to not be too fast
        LimitVelocity();

        DeadCheck();

        if(transform.position.z < PlayerObj.transform.position.z+2 && !isStageOver){
            PlayerObj.GetComponent<Player>().beginToCount = true;
        }
    }


    void LimitVelocity()
    {
        if (rb.velocity.x > 5) rb.velocity = new Vector3(5, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.x < -5) rb.velocity = new Vector3(-5, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.y > 5) rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
        if (rb.velocity.y < -5) rb.velocity = new Vector3(rb.velocity.x, -5, rb.velocity.z);

        if (rb.velocity.z > 5) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 5);
        if (rb.velocity.z < -5) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -5);

        if (rb.velocity.z > -3) rb.AddForce(addForce);
    }



    void DeadCheck()
    {   

        // When the stage is over, all obstacles are blown into the sky.
        if (stageNumber != GamePlayManager.CURRENT_STAGE && !isStageOver)
        {
            isStageOver = true;

            rb.isKinematic = false;
            rb.velocity = new Vector3(Random.Range(-10, 10), 20, Random.Range(-10, 10));
            StartCoroutine(destroyObj());
        }

        //If the obstacle gets out of the camera, it will be destroyed.
        if (PlayerObj.transform.position.z - transform.position.z > 11)
        {
            Destroy(this.transform.gameObject);
        }
    }




    void Rotation()
    {
        transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
    }



    void Move()
    {
        MoveAngle_X += Time.deltaTime * Move_X_Speed;
        float posX = Mathf.Sin(MoveAngle_X) * Move_X_Distance;

        MoveAngle_Y += Time.deltaTime * Move_Y_Speed;
        float posY = Mathf.Sin(MoveAngle_Y) * Move_Y_Distance;

        MoveAngle_Z += Time.deltaTime * Move_Z_Speed;
        float posZ = Mathf.Sin(MoveAngle_Z) * Move_Z_Distance;

        transform.position = startPos + new Vector3(posX, posY, posZ);
    }




    void InitStartAngle()
    {
        float angleX = 0;
        float angleY = 0;
        float angleZ = 0;

        if (randomAngle.x == true) angleX = Random.Range(0, 90);
        if (randomAngle.y == true) angleY = Random.Range(-45, 45);
        if (randomAngle.z == true) angleZ = Random.Range(0, 180);
      
        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
    }


    void InitStartPosition()
    {
        float positionX = 0;
        if (randomPositionX == true)
        {
            int randomInt = Random.Range(1, 4);
            if (randomInt == 1) positionX = -2.0f;
            if (randomInt == 2) positionX = 0f;
            if (randomInt == 3) positionX = 2.0f;

            transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
            startPos = transform.position;
        }

        if (randomPositionY == true)
        {
            float positionY = 0;
            int randomInt = Random.Range(1, 3);
            if (randomInt == 1) positionY = 0.7f;
            if (randomInt == 2) positionY = 4f;

            transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void InitRotationSpeed()
    {
        rotationSpeedX = Random.Range(RotationSpeedX_Min, RotationSpeedX_Max);
        rotationSpeedY = Random.Range(RotationSpeedY_Min, RotationSpeedY_Max);
        rotationSpeedZ = Random.Range(RotationSpeedZ_Min, RotationSpeedZ_Max);
    }



    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(this.transform.parent.gameObject);
        yield break;
    }


}
