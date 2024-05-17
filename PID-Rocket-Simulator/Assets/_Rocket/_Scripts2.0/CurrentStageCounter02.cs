using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStageCounter02 : MonoBehaviour {


    public int currentStage = 0;
    public bool automation = false;
    bool executeFlying = false;
    bool flyingUpBool = false;
    bool stepActivated = false;
    bool goToNewTarget = false;
    

    [SerializeField] Transform Target;

    LastRotationPID02 myLastRPID;
    LastRotationMotor02 myLastMotor;
    RotationPID02 myRPID;
    RotationMotor02 myRotationMotor;
    PidController02 myPC;

    Rigidbody rb;


    private void Start()
    {
        myLastRPID = GetComponent<LastRotationPID02>();
        myLastMotor = GetComponent<LastRotationMotor02>();
        myRPID = GetComponent<RotationPID02>();
        myRotationMotor = GetComponent<RotationMotor02>();
        myPC = GetComponent<PidController02>();
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {        
        AreWeFlyingUpOrDown();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            goToNewTarget = true;
        }
    }

    private void FixedUpdate()
    {

        if (executeFlying && flyingUpBool)
        {
            FlyingUp();
        }
        else if (executeFlying && flyingUpBool == false)
        {
            FlyingDown();
        }
    }

    private void AreWeFlyingUpOrDown()
    {
        float errorY = Target.transform.position.y - this.transform.position.y;
        if (executeFlying == false && errorY > 0)
        {
            currentStage = 0;
            executeFlying = true;
            flyingUpBool = true;
        }
        else if (executeFlying == false && errorY < 0)
        {
            currentStage = 0;
            executeFlying = true;
            flyingUpBool = false;
        }
    }

    private void FlyingUp()
    {
        switch (currentStage)
        {
            case 3:

                /* VISS RAKETTEN 
                 * 
                 * 
                 * 
                 */ 
                break;

            case 2:

                /* VISS VI KOMMER NÆRME NOK VIL X- OG Z-PID BYTTES UT TIL DE TREGERE PIDENE SLIK AT EIN KAN KOMME TRYGGERE, MEN SEINARE FRAM
                 * 
                 * 
                 * 
                 */ 
                break;

            case 1:
                print("STAGE 1");
                if (stepActivated == false)
                {
                    stepActivated = true;
                    stepActivated = false;
                    myLastRPID.enabled = false;
                    myLastMotor.enabled = false;
                    myRPID.enabled = false;
                    myRotationMotor.enabled = false;
                }
                //AKTIVER Y-ROTATION PID

                //ACTIVATE Z PID

                //ACTIVATE NEW X PID

                // IF (ALL ABOVE = ACTIVE) { CURRENTSTAGE++}

                /* DEAKTIVERER ROTATION PIDS I X OG Z
                 * BEGYNNER Å ROTERE SLIK AT X PEIKER MOT TARGET I XZ-PLAN
                 * NÅR X PEIKER RETT AKTIVERES ROTATION PID I Z-AKSEN SEG, MEN IKKJE I X-AKSEN. DER TAR EIN NY PID OVER
                 * VISS NYPIDX ER AKTIVERT/KLAR SÅ VIL PIDY AKTIVERES (KANSKJE SETTE SITT NYE TARGET) SLIK AT DEN BEGYNNER Å STIGE
                 * 
                 */


                break;

            //Default er 0
            default:
                print("IN DEFAULT MODE");
                if (stepActivated == false)
                {
                    stepActivated = true;
                    myLastRPID.enabled = true;
                    myLastMotor.enabled = true;
                    myRPID.enabled = true;
                    myRotationMotor.enabled = true;
                }

                /* MÅ HA PID CONTROLLERS SOM HOLDER DEN I RO VED TARGET, X-, Y-, OG Z-PID
                 * ition.y < 20 || (errorInY < 50 && errorInY > -50))
                * VISS NY TARGET AKTIVERES SÅ VIL ME GÅ TIL STAGE 1 VISS VELOCITY I X=Z=0 (NÆRME 0)                
                */
                if (goToNewTarget == true && rb.velocity.x < 1 && rb.velocity.x > -1 && rb.velocity.z < 1 && rb.velocity.z > -1 && rb.velocity.y < 1 && rb.velocity.y > -1)
                {
                    stepActivated = false;
                    currentStage++;
                    
                }

                //LAGE EIN KLOKKE SOM TELLER NED OG GJØR TING AUTOMATISK IDE!

                break;
        }
    }

    private void FlyingDown()
    {
        switch (currentStage)
        {
            case 3:

                break;

            case 2:

                break;

            case 1:

                break;

            default:


                break;
        }
    }

}
