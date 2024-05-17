using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRotationMotor02 : MonoBehaviour
{

    Motor02 motor;
    Rigidbody myR;
    LastRotationPID02 myRotationPid;

    [SerializeField] Transform target;
    public float sideThrust;

    Vector3 currentTargetPos;

    float localDegrees, degreesPercentage, percentageOutput;
    bool isOutputPositive = false;
    float errorInY;

    private void Start()
    {
        motor = GetComponent<Motor02>();
        myRotationPid = GetComponent<LastRotationPID02>();
        myR = GetComponent<Rigidbody>();
        currentTargetPos = target.transform.position;
        myRotationPid.newTargetPos = true;

    }

    private float MaxRotationAllowed()
    {
        //DETTE ER MAX MED GRADER SOM MAN KAN HA FOR Å AKKURAT KUNNE MOTVIRKE GRAVITASJONSKRAFTEN SLIK AT
        //SUMMEN AV KREFTENE I Y RETNING = 0
        float degree = Mathf.Atan2(9.81f, (motor.max - 9.81f)) * 180 / 3.14f;
        degree = 90 - degree;
        return degree;
    }

    private void Update()
    {
        NewTargetPosition(); //Sjekker om target har endret posisjon
    }


    private void FixedUpdate()
    {
        myRotationPid.PidCalculation(target.transform.position, this.transform.position, myR.velocity.x);


        //EG VIL OUTPUTEN SKAL BLI BRUKT TIL Å KOMMA SEG SÅ NÆRME TIL VEKTOREN TIL TARGET, MEN GRADENE KAN IKKJE GÅ OVER MAXROTATIONALLOWED


        PercentageCalculator(myRotationPid.GetOutput());
        RotateRocket();


    }

    private void PercentageCalculator(float output)
    {
        //DETTE GIR OUTPUT I PROSENT, PROSENT ER ALLTID EIT POSITIVT TAL
        if (output > 0)
        {
            isOutputPositive = true;

            percentageOutput = output / 10;

        }
        else if (output < 0)
        {
            isOutputPositive = false;

            percentageOutput = -output / 10;

        }
    }

    //DETTE GÅR DIREKTE INN PÅ ROTASJONEN OG ENDRER DEN DER
    private void RotateRocket()
    {

        errorInY = target.transform.position.y - this.transform.position.y;
        //currentRotation% skal bli output%

        if (isOutputPositive)
        {

            UseRightThrust();

            //GÅ TIL HØGRE

        }
        else if (!isOutputPositive)
        {

            UseLeftThrust();


        }
    }

    //DETTE BRUKER FYSIKKMOTOREN TIL UNITY
    private void UseRightThrust()
    {
        //LINJEN UNDER ER FOR Å FÅ OPP NEGATIV VINKELVERDI
        localDegrees = this.transform.localEulerAngles.z;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;
        degreesPercentage = localDegrees / MaxRotationAllowed();

        float maxRotation = -MaxRotationAllowed();
        if (this.transform.position.y < 20 || (errorInY < 50 && errorInY > -50))
        {
            maxRotation += 20;
        }

        if (localDegrees < maxRotation)
        {
            //return;

            //this.transform.rotation = Quaternion.Euler(0, 0, -MaxRotationAllowed());

            Quaternion originalRot = transform.rotation;


            this.transform.rotation = Quaternion.Euler(originalRot.eulerAngles.x, originalRot.eulerAngles.y, maxRotation);

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

                this.transform.Rotate(-Vector3.forward * sideThrust * rotationPower * Time.fixedDeltaTime, Space.Self);


            }



        }

    }

    private void UseLeftThrust()
    {
        //-MaxRotationAllowed() fordi det er andre vegen
        localDegrees = this.transform.localEulerAngles.z;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;
        degreesPercentage = localDegrees / MaxRotationAllowed();

        float maxRotation = MaxRotationAllowed();
        if (this.transform.position.y < 20 || (errorInY < 50 && errorInY > -50))
        {
            maxRotation -= 20;
        }

        if (localDegrees > maxRotation)
        {
            //return;
            //this.transform.rotation = Quaternion.Euler(0, 0, MaxRotationAllowed());

            Quaternion originalRot = transform.rotation;

            this.transform.rotation = Quaternion.Euler(originalRot.eulerAngles.x, originalRot.eulerAngles.y, maxRotation);

        }
        else
        {

            //percentageOutput = myRotationPid.GetOutput() / 10;

            if (percentageOutput > degreesPercentage)
            {
                float rotationPower = Mathf.Abs(myRotationPid.GetOutput());
                if (float.IsNaN(rotationPower))
                {
                    return;
                }



                this.transform.Rotate(Vector3.forward * sideThrust * rotationPower * Time.fixedDeltaTime, Space.Self);

            }



        }
    }



    private void NewTargetPosition()
    {
        if (currentTargetPos != target.transform.position)
        {
            currentTargetPos = target.transform.position;
            myRotationPid.newTargetPos = true;
        }
    }



}
