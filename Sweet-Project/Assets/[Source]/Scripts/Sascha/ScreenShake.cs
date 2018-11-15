using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenShake : MonoBehaviour
{

    private Camera myCamera;
    public float maxFOV = 1;
    public float minFOV = 1;
    public bool activate;
    public float timer;
    public float timeLimit = 0.2f;
    public bool debug;

    void Awake ()
    {
        myCamera = Camera.main;
    }

    void Update ()
    {
        if(debug)
        {
            ResetTimer();
            debug = false;
        }

        if(timer < 10)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timeLimit)
        {
            activate = false;
        }
        else activate = true;

        if(activate)
        {
            ShakeCamera();
        }
    }

    void ShakeCamera()
    {
        myCamera.fieldOfView = Random.Range(myCamera.fieldOfView + maxFOV, myCamera.fieldOfView - minFOV);
    }

    void ResetTimer ()
    {
        timer = 0.0f;
    }
}