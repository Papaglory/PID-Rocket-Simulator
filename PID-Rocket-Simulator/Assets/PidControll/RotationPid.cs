using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPid : MonoBehaviour {


    Motor2 myMotor;
    RotationMotor myRotationMotor;

    [Header("PID constant")]
    [SerializeField] float Kp, Kd, Ki;
    [SerializeField] Transform target;
    [Tooltip("Dette kan me se på som farten til rotasjonen")]
    [SerializeField] float min = -1, max = 1;
    [SerializeField] float maxThrustForSailing;
    [SerializeField] float distanceToActivateIntegral;

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
        errorWhenNewTarget = Mathf.Abs((target.position.x - this.transform.position.x));
        myMotor = GetComponent<Motor2>();
        myRotationMotor = GetComponent<RotationMotor>();

    }

    public void PidCalculation(Vector3 target, Vector3 currentValue, float currentVelocity)
    {
        Vector3 error = target - currentValue;
        float fError = error.z;

        float constant = 0;
        
        float errorHeight = error.y;



        //imaginaryPosZ virker bare når det er til høgre
        if (newTargetPos)
        {
            imaginaryPosZ = target.z/4;

            imaginaryPosZ = target.z - (target.z - currentValue.z) * sailingValue;
            
            

            imaginaryPosZToRight = target.z + (currentValue.z - target.z) * sailingValue;

            newTargetPos = false;
        }

        if (currentValue.z > target.z)
        {
            targetIsToRight = false;

        }
        if (currentValue.z < target.z)
        {
            targetIsToRight = true;

        }

        if (targetIsToRight)
        {
            if (errorHeight < -50 && currentValue.z < imaginaryPosZ)
            {
                myMotor.sailingThrust = maxThrustForSailing;
                myMotor.MotorForSide();
            }
            else
            {
                myMotor.sailingThrust = 0;
            }

        }
        else if (!targetIsToRight)
        {
            if (errorHeight < -50 && currentValue.z > imaginaryPosZToRight)
            {
                myMotor.sailingThrust = maxThrustForSailing;
                myMotor.MotorForSide();

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
            constant = Mathf.Abs(Mathf.Pow(fError, (9.25f/10f)) / (errorHeight));

        }

        if (Mathf.Abs(fError) > distanceToActivateIntegral)
        {
            integral = 0;
        }

        if (integral == fError)
        {
            integral = 0;
        }
        integral += fError * Time.fixedDeltaTime;


        float errorSlope = fError - oldError;
        errorSlope = errorSlope / Time.fixedDeltaTime;
        /*
        if (errorSlope * Kd > fError*Kp)
        {
            myRotationMotor.sideThrust = 15;
        }
        else
        {
            myRotationMotor.sideThrust = 10;
        }
        */

        output = (integral * Ki) + (errorSlope * Kd) + (fError * Kp * constant);
        output = Mathf.Clamp(output, min, max);

        //print(output);

        oldError = fError;
    }


    public float GetOutput() { return output; }


}





