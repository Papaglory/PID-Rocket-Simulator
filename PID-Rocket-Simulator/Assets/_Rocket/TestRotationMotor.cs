using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationMotor : MonoBehaviour
{
    Motor02 motor;
    Rigidbody myR;
    TestRotationPid myRotationPid;

    [SerializeField] Transform target;
    public float sideThrust;

    float errorInY;
    float localDegrees, degreesPercentage, percentageOutput;
    bool isOutputPositive = false;
    float outputRetrieved;
    float currentMaxMotorthrust;
    float maxRot;

    float lastVelocity;
    float acc;

    float errorInYWhenNewTarget;
    bool newTargetPos = false;
    bool increasingHeight = false;

    public bool activateNow = false;

    Vector3 currentTargetPos;

    private void Start()
    {
        myRotationPid = GetComponent<TestRotationPid>();
        motor = GetComponent<Motor02>();
        myR = GetComponent<Rigidbody>();
        currentMaxMotorthrust = motor.max;
        maxRot = MaxRotationAllowed();
        currentTargetPos = Vector3.zero;


    }

    private void Update()
    {
        // Check if the target has moved

        NewTargetPosition();

        if (newTargetPos)
        {
            newTargetPos = false;
            errorInYWhenNewTarget = target.transform.position.y - this.transform.position.y;
        }

        if (motor.max != currentMaxMotorthrust)
        {
            currentMaxMotorthrust = motor.max;
            maxRot = MaxRotationAllowed();
        }
    }

    private void NewTargetPosition()
    {
        if (currentTargetPos != target.transform.position)
        {
            currentTargetPos = target.transform.position;
            newTargetPos = true;

            // Check if the target is above the rocket
            if (target.transform.position.y - this.transform.position.y > 0)
            {
                increasingHeight = true;
            }
        }
    }


    private float MaxRotationAllowed()
    {
        // This is the maximum degree allowed given the maximum motor thrust to have the rocket hover.
        float degree = Mathf.Atan2(9.81f, (motor.max - 9.81f)) * 180 / 3.14f;
        degree = 90 - degree;
        return degree;
    }



    private void FixedUpdate()
    {
        myRotationPid.PidCalculation(target.transform.position, this.transform.position);

        outputRetrieved = myRotationPid.GetPercentageOutput();
        RotateRocket();
    }

    private void RotateRocket()
    {
        errorInY = target.transform.position.y - this.transform.position.y;

        localDegrees = this.transform.localEulerAngles.z;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;

        float maxRotation = maxRot;
        if (this.transform.position.y < 20 || (errorInY < 50 && errorInY > -50) && increasingHeight == false)
        {
            maxRotation -= 20;
        }

        if (increasingHeight == true && activateNow == true)
        {
            float percentageTraveled = this.transform.position.y / errorInYWhenNewTarget;
            if (percentageTraveled > 0.8f)
            {
                increasingHeight = false;
            }
            maxRotation = maxRot * percentageTraveled;//Height * percentage traveled in y

        }


        float wantedRotation;
        float currentRotation;
        currentRotation = localDegrees;
        
        wantedRotation = outputRetrieved * maxRotation;
        float rotationToRotate = wantedRotation - currentRotation;



        this.transform.Rotate(0, 0, rotationToRotate * Time.fixedDeltaTime * sideThrust, Space.Self);

    }

    private void UseRightThrust()
    {
        localDegrees = this.transform.localEulerAngles.x;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;
        degreesPercentage = localDegrees / maxRot;

        float maxRotation = -maxRot;
        if (this.transform.position.y < 20 || (errorInY < 50 && errorInY > -50))
        {
            maxRotation += 20;
        }

        if (localDegrees < maxRotation)
        {


            Quaternion originalRot = transform.rotation;

            this.transform.rotation = Quaternion.Euler(maxRotation, originalRot.eulerAngles.y, originalRot.eulerAngles.z);

        }
        else
        {





            if (percentageOutput > -degreesPercentage)
            {
                float rotationPower = Mathf.Abs(myRotationPid.GetOutput());

                if (float.IsNaN(rotationPower))
                {
                    return;
                }

                this.transform.Rotate(-Vector3.right * sideThrust * rotationPower * Time.fixedDeltaTime, Space.Self);


            }



        }

    }

    private void UseLeftThrust()
    {
        localDegrees = this.transform.localEulerAngles.x;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;
        degreesPercentage = localDegrees / maxRot;

        float maxRotation = maxRot;
        if (this.transform.position.y < 20 || (errorInY < 50 && errorInY > -50))
        {
            maxRotation -= 20;
        }

        if (localDegrees > maxRotation)
        {

            Quaternion originalRot = transform.rotation;

            this.transform.rotation = Quaternion.Euler(maxRotation, originalRot.eulerAngles.y, originalRot.eulerAngles.z);

        }
        else
        {


            if (percentageOutput > degreesPercentage)
            {
                float rotationPower = Mathf.Abs(myRotationPid.GetOutput());
                if (float.IsNaN(rotationPower))
                {
                    return;
                }



                this.transform.Rotate(Vector3.right * sideThrust * rotationPower * Time.fixedDeltaTime, Space.Self);

            }



        }
    }






}
