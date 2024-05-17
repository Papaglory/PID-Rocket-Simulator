using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidController02 : MonoBehaviour
{
    [Header("PID constant")]
    [SerializeField] float Kp, Kd, Ki; // Seems like Kp=0.76 and Kd = 0.44 are good values with gravitation and mass = 1. Ki =0.2 looks good with 0.6 drag in the rigidbody.
    [SerializeField] bool isClamp;
    [Tooltip("Clamp with min = 0 and max = 1 being the default. Will then get the percentage.")]
    public float min, max;
    //Recommended values: ISCLAMP = TRUE, MIN = 0, MAX = 1

    [SerializeField] Transform target;

    float integral = 0, oldError = 0, output;


    public bool newTargetPos;
    public float errorWhenNewTarget;



    private void Start()
    {
        errorWhenNewTarget = Mathf.Abs((target.position.y - this.transform.position.y));

    }


    public void PidCalculation(Vector3 target, Vector3 currentValue, float currentVelocity)
    {
        Vector3 error = target - currentValue;
        float fError = error.y;
        float constant;

        float fErrorForConstant = 0;
        float absError = Mathf.Abs(fError);


        float sqrtConstantUp, sqrtConstantDown;


        if (newTargetPos)
        {
            errorWhenNewTarget = absError;
            integral = 0;

            newTargetPos = false;

        }

        if (errorWhenNewTarget < 1000)
        {
            errorWhenNewTarget = 1000;
        }



        if (fError <= 0)
        {

            // Negative


            sqrtConstantDown = (400000f - Mathf.Sqrt((400000f * 400000f) - 4f * 400000f * (101000f - errorWhenNewTarget))) / (2f * 400000f);

            fErrorForConstant = -Mathf.Pow(absError, sqrtConstantDown);
            constant = Mathf.Abs(currentVelocity / fErrorForConstant);
        }
        else
        {
            // Positive


            sqrtConstantUp = (400000f - Mathf.Sqrt((400000f * 400000f) - 4f * 400000f * (101000f - errorWhenNewTarget))) / (2f * 400000f);
            fErrorForConstant = Mathf.Pow(fError, sqrtConstantUp * 0.75f);

            // Constant that breaks the closer we get
            constant = Mathf.Abs(currentVelocity / fErrorForConstant);
        }



        integral += fError * Time.fixedDeltaTime;

        float errorSlope = fError - oldError;

        errorSlope = errorSlope / Time.fixedDeltaTime;

        output = (integral * Ki) + (errorSlope * Kd * constant) + (fError * Kp);

        if (isClamp)
        {
            output = Mathf.Clamp(output, min, max);
        }

        oldError = fError;
    }

    public void EnableClamp(float newMin, float newMax)
    {
        isClamp = true;
        min = newMin;
        max = newMax;
    }

    public float GetOutput() { return output; }

    public void DisableClamp() { isClamp = false; }



}
