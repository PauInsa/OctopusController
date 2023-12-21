using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MagnusEffect : MonoBehaviour
{
    public Transform ball;
    public List<Transform> predictedSpheres;
    public List<Transform> shotSpheres;

    public Slider forceSlider;
    public Slider effectSlider;

    private float gravity = 9;

    private bool startForceBarMovement;
    private float timeStartForceBar;
    public float forceBarSpeed;
    



    // Start is called before the first frame update
    void Start()
    {
        startForceBarMovement = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
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



        updateParabola();
    }

    void updateParabola()
    {
        for(int i = 0; i < predictedSpheres.Count; i++)
        {
            Vector3 newPos;

            float proportion = (((float)i + 1.0f) / (float)predictedSpheres.Count)*2;

            newPos.x = ball.position.x;
            newPos.z = ball.position.z - forceSlider.value/20 * proportion;
            newPos.y = ball.position.y + forceSlider.value / 70 * proportion - gravity * Mathf.Pow(proportion,2);


            predictedSpheres[i].position = newPos;
        }

        for(int i = 0;i < shotSpheres.Count;i++)
        {
            Vector3 newPos;

            float proportion = (((float)i + 1.0f) / (float)predictedSpheres.Count) * 2;

            newPos.x = predictedSpheres[i].position.x;
            newPos.z = predictedSpheres[i].position.z;
            newPos.y = predictedSpheres[i].position.y;


            shotSpheres[i].position = newPos;
        }
    }
}
