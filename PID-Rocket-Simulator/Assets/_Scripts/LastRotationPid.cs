using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRotationPid : MonoBehaviour
{


    Motor2 myMotor;
    LastRotationMotor myRotationMotor;

    [Header("PID constant")]
    [SerializeField] float Kp, Kd, Ki;
    [SerializeField] Transform target;
    [Tooltip("Dette kan me se på som farten til rotasjonen")]
    [SerializeField] float min = -1, max = 1;
    [SerializeField] float maxThrustForSailing;

    float integral = 0, oldError = 0, output;
    float absError;

    public bool newTargetPos;

    public float errorWhenNewTarget;

    bool targetIsToRight;

    float imaginaryPosZ;
    float imaginaryPosZToRight;

    [Tooltip("Verdi som avgjør hvor lenge raketten skal seile. Verdi mellom 0 og 1")]
    [SerializeField] float sailingValue = 0.75f;

    private void Start()
    {
        errorWhenNewTarget = Mathf.Abs((target.position.z - this.transform.position.z));
        myMotor = GetComponent<Motor2>();
        myRotationMotor = GetComponent<LastRotationMotor>();

    }

    public void PidCalculation(Vector3 target, Vector3 currentValue, float currentVelocity)
    {
        Vector3 error = target - currentValue;
        float fError = error.x;

        float constant = 0;

        float errorHeight = error.y;



        //imaginaryPosZ virker bare når det er til høgre
        if (newTargetPos)
        {
            imaginaryPosZ = target.x / 4;

            imaginaryPosZ = target.x - (target.x - currentValue.x) * sailingValue;



            imaginaryPosZToRight = target.x + (currentValue.x - target.x) * sailingValue;

            newTargetPos = false;
        }

        if (currentValue.z > target.x)
        {
            targetIsToRight = false;

        }
        if (currentValue.z < target.x)
        {
            targetIsToRight = true;

        }

        if (targetIsToRight)
        {
            if (errorHeight < -50 && currentValue.x < imaginaryPosZ)
            {
                myMotor.sailingThrust = maxThrustForSailing;
                //myMotor.MotorForSide();
            }
            else
            {
                myMotor.sailingThrust = 0;
            }

        }
        else if (!targetIsToRight)
        {
            if (errorHeight < -50 && currentValue.x > imaginaryPosZToRight)
            {
                myMotor.sailingThrust = maxThrustForSailing;
                //myMotor.MotorForSide();

            }
            else
            {
                myMotor.sailingThrust = 0;
            }
        }




        //Dette vil gjøre raketten meir nøyaktig når den nærmer seg målet
        if (fError < 50 && errorHeight < 50)
        {
            constant = 1;
        }
        else
        {
            constant = Mathf.Abs(Mathf.Pow(fError, (9.25f / 10f)) / (errorHeight));

        }

        integral += fError * Time.fixedDeltaTime;

        float errorSlope = fError - oldError;
        errorSlope = errorSlope / Time.fixedDeltaTime;


        output = (integral * Ki) + (errorSlope * Kd) + (fError * Kp * constant);
        output = Mathf.Clamp(output, min, max);

        //print(output);

        oldError = fError;
    }


    public float GetOutput() { return output; }

}
