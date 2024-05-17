using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLastRotationMotor : MonoBehaviour
{
    Motor02 motor;
    Rigidbody myR;
    TestLastRotationPid myRotationPid;

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

    public float errorInYWhenNewTarget;
    bool newTargetPos = false;
    bool increasingHeight = false;
    bool decreaseHeight = false;

    public bool activateMaShit = false;

    Vector3 currentTargetPos;

    private void Start()
    {
        myRotationPid = GetComponent<TestLastRotationPid>();
        motor = GetComponent<Motor02>();
        myR = GetComponent<Rigidbody>();
        currentMaxMotorthrust = motor.max;
        maxRot = MaxRotationAllowed();
        currentTargetPos = Vector3.zero;


    }

    private void Update()
    {
        //Sjekker om target har beveget på seg

        NewTargetPosition();
    
        //Går bare inn her viss det er ein ny posisjon til target
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
        /*
        acc = (myR.velocity.x - lastVelocity) / Time.deltaTime;
        lastVelocity = myR.velocity.x;
        print("ACCELERATION " + acc);*/
        //print("DETTE ER MAXROT: " + maxRot);
        //print(outputRetrieved * maxRot);
        //print("OUTPUT " + myRotationPid.GetPercentageOutput());
    }

    private void NewTargetPosition()
    {
        if (currentTargetPos != target.transform.position)
        {
            currentTargetPos = target.transform.position;
            newTargetPos = true;

            //Dette er for å sjekke at target er over slik at maxrotation blir restricted
            if (target.transform.position.y - this.transform.position.y > 0)
            {
                increasingHeight = true;
            }
            
        }
    }


    private float MaxRotationAllowed()
    {
        //DETTE ER MAX MED GRADER SOM MAN KAN HA FOR Å AKKURAT KUNNE MOTVIRKE GRAVITASJONSKRAFTEN SLIK AT
        //SUMMEN AV KREFTENE I Y RETNING = 0
        float degree = Mathf.Atan2(9.81f, (motor.max - 9.81f)) * 180 / 3.14f;
        degree = 90 - degree;
        return degree;
    }



    private void FixedUpdate()
    {
        myRotationPid.PidCalculation(target.transform.position, this.transform.position);


        //EG VIL OUTPUTEN SKAL BLI BRUKT TIL Å KOMMA SEG SÅ NÆRME TIL VEKTOREN TIL TARGET, MEN GRADENE KAN IKKJE GÅ OVER MAXROTATIONALLOWED


        outputRetrieved = myRotationPid.GetPercentageOutput();
        RotateRocket();
        //FixPosOnWayDown();

    }




    //DETTE GÅR DIREKTE INN PÅ ROTASJONEN OG ENDRER DEN DER
    private void RotateRocket()
    {
        errorInY = target.transform.position.y - this.transform.position.y;

        //currentRotation% skal bli output%

        /*
         * OUTPUT BLIR OM TIL EIN PROSENTVERDI DER 100% ER MAXROTATIONALLOWED, MENS 0% ER -MAXROTATIONALLOWED, 50% = 0 DEGREES
         * LAGER EIN POSITIV OUTPUT OG EIN NEGATIV OUTPUT, TROR VIRKER BEDRE
         * TA PROSENTEN OG DEL MED 100 FOR Å FÅ DET MELLOM 0 OG 1. EIN KAN DA GANGE MED DENNE VERDIEN. BLIR DET 1 SÅ ER DET 48 GRADER, 0 ER 0 GRADER. -1 ER -48 GRADER OG SÅ VIDERE
         * 
         * 
         */
        //Wanted rotation = outputRetrieved (EIN VERDI MELLOM -1 OG 1) * maxrot
        /*
         * Får me 1 altså 100% så vil den gå maxRot i grader. Da er dette ønsket rotasjon og den vil rotere der med rotationSpeed
         * 
         * 
         * 
         * 
         * wantedRotation - currentRotation
         * Dersom den er wantedRotation så vil den vere 0 i transform.rotate( )
         * den vil da ikkje rotere meir, eventuelt rotere tilbake viss den overshoote
         * 
         * 
         * 
         */

        localDegrees = this.transform.localEulerAngles.x;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;

        float maxRotation = maxRot;
        if (this.transform.position.y < 20 || (errorInY < 50 && errorInY > -50) && increasingHeight == false)
        {
            maxRotation -= 20;
        }

        if (increasingHeight == true && activateMaShit == true)
        {
            float percentageTraveled = this.transform.position.y / errorInYWhenNewTarget;
            if (percentageTraveled > 0.8f)
            {
                increasingHeight = false;
            }
            maxRotation = maxRot * percentageTraveled;//Height * percentage traveled in y


        }

        //print("EULER " + localDegrees);
        
        float wantedRotation;
        float currentRotation;
        currentRotation = localDegrees;
        //desiredRotation = outputRetrieved * maxRot;

        //wantedRotation = this.transform.rotation.eulerAngles.z - outputRetrieved * maxRot;
        wantedRotation = outputRetrieved * maxRotation;
        float rotationToRotate = wantedRotation - currentRotation;


        /*
        print("wantedRotation " + wantedRotation);
        print("currentRotation " + currentRotation);
        print("rotationToRotate " + rotationToRotate);
        */

        //this.transform.Rotate(0, 0, outputRetrieved * maxRot * sideThrust, Space.Self);


        this.transform.Rotate(rotationToRotate * Time.fixedDeltaTime * sideThrust, 0, 0, Space.Self);





        // this.transform.Rotate(-Vector3.right * sideThrust * rotationPower * Time.fixedDeltaTime, Space.Self);


        /*

        if (isOutputPositive)
        {



            //GÅ TIL HØGRE
            UseLeftThrust();
        }
        else if (!isOutputPositive)
        {



            UseRightThrust();
        }
        */
    }




    //DETTE BRUKER FYSIKKMOTOREN TIL UNITY
    private void UseRightThrust()
    {
        //LINJEN UNDER ER FOR Å FÅ OPP NEGATIV VINKELVERDI
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
            //return;

            //this.transform.rotation = Quaternion.Euler(-MaxRotationAllowed(), 0, 0);

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
        //-MaxRotationAllowed() fordi det er andre vegen
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
            //return;

            //this.transform.rotation = Quaternion.Euler(MaxRotationAllowed(), 0, 0);
            Quaternion originalRot = transform.rotation;

            this.transform.rotation = Quaternion.Euler(maxRotation, originalRot.eulerAngles.y, originalRot.eulerAngles.z);

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



                this.transform.Rotate(Vector3.right * sideThrust * rotationPower * Time.fixedDeltaTime, Space.Self);

            }



        }
    }






}
