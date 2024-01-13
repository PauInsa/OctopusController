using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MagnusEffect : MonoBehaviour
{
    public Transform ball;
    public Transform target;
    public List<Transform> predictedSpheres;
    public List<Transform> shotSpheres;

    public Slider forceSlider;
    public Slider effectSlider;

    private float gravity = 9;

    private bool startForceBarMovement;
    private float timeStartForceBar;
    public float forceBarSpeed;

    public float ballSpeed;
    private float timeStartBall;
    bool ballStarted;
    bool ballStopped;

    Vector3 initBallPos;
    



    // Start is called before the first frame update
    void Start()
    {
        startForceBarMovement = false;

        ballStarted = false;
        ballStopped = false;

        initBallPos = ball.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ball.position = initBallPos;
            startForceBarMovement = true;
            timeStartForceBar = Time.time;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            startForceBarMovement = false;
        }

        if(startForceBarMovement)
        {
            float newValue;

            float timer = (Time.time - timeStartForceBar)/forceBarSpeed;

            if(timer >= 1.0f)
            {
                timer = (timeStartForceBar + 2*forceBarSpeed - Time.time)/forceBarSpeed;

                Debug.Log(timer);

                if (timer <= 0.0f)
                    timeStartForceBar = Time.time;
            }

            newValue = forceSlider.maxValue * timer;
            forceSlider.value = newValue;

            
        }


        if(ballStarted)
            UpdateMoveBall();
        
        if(startForceBarMovement)
            UpdateParabola();
    }

    void UpdateParabola()
    {
        for(int i = 0; i < predictedSpheres.Count; i++)
        {
            Vector3 newPos;

            float proportion = (((float)i + 1.0f) / (float)predictedSpheres.Count)*2;

            newPos.x = initBallPos.x;
            newPos.z = initBallPos.z - forceSlider.value/20 * proportion;
            newPos.y = initBallPos.y + forceSlider.value/70 * proportion - gravity * Mathf.Pow(proportion,2);


            predictedSpheres[i].position = newPos;
        }


        //Magnus effect formula: F=p*v*w*A
        //F = lift force,
        //p = air density,
        //v = velocity of the object,
        //A = cross-sectional area of the object


        for (int i = 0;i < shotSpheres.Count;i++)
        {
            Vector3 newPos;

            //Densitat de l'aire 1.2Kg/m^3
            float p = 1.2f;

            //Slider de forca
            float v = forceSlider.value;

            //Slider de l'effecte
            float w = effectSlider.value;

            //Diametre de la pilota.
            float A = 0.001680421f*2;

            float magnussForce = p*v*w*A;

            float proportion = (((float)i + 1.0f) / (float)shotSpheres.Count) * 2;

            newPos.x = predictedSpheres[i].position.x + (magnussForce/50) * proportion;
            newPos.z = predictedSpheres[i].position.z;
            newPos.y = predictedSpheres[i].position.y;


            shotSpheres[i].position = newPos;
        }
    }

    public void StartBallMovement(bool stop)
    {
        ballStopped = !stop;
        ballStarted = true;

        timeStartBall = Time.time;
    }

    private void UpdateMoveBall()
    {

        float timer = ((Time.time - timeStartBall) / ballSpeed)*2;

        //Densitat de l'aire 1.2Kg/m^3
        float p = 1.2f;

        //Slider de forca
        float v = forceSlider.value;

        //Slider de l'effecte
        float w = effectSlider.value;

        //Diametre de la pilota.
        float A = 0.001680421f * 2;

        float magnussForce = p * v * w * A;

        Vector3 newPos= ball.position;

        newPos.x = initBallPos.x + (magnussForce / 50f) * timer;
        newPos.z = initBallPos.z - forceSlider.value / 20f * timer;
        newPos.y = initBallPos.y + forceSlider.value / 70f * timer - gravity * Mathf.Pow(timer, 2);

        ball.position = newPos;

        if(ballStopped)
        {
            newPos.z = -72f;
            target.position = newPos;
        }


        Debug.Log(initBallPos);


        if(ball.position.z <= -70.1f && ballStopped || (ball.position-initBallPos).magnitude >=100f)
        {
            ballStarted = false;
        }

    }
}
