using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFallingPath : MonoBehaviour {

    TestLastRotationMotor rotationMotor;
    Motor02 motor;
    [SerializeField] Transform target;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject smoke;

    PidController02 myP = null;

    public bool goingDownForGood = false;
    bool decreaseHeight = false;
    bool startTimer = false;    
    int timeAdd = 1;
    float timer = 0;
    public bool executeFallBurn = false;
    float currentSpeedInXZ = 0;
    float velocityToReach = 0;
    float heightToWidthRatio = 0;
    float timerShutOff = 0;
    bool resetBurn = false;
    public bool usingMotor = false;

    // Use this for initialization
    void Start () {
        rotationMotor = GetComponent<TestLastRotationMotor>();
        motor = GetComponent<Motor02>();
        myP = GetComponent<PidController02>();
        startTimer = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        FixPosOnWayDown();
	}

    private void Update()
    {
        if (target.transform.position.y - this.transform.position.y < -100)
        {
            decreaseHeight = true;
        }

        if (myP.newTargetPos == true)
        {
            goingDownForGood = false;
        }

    }

    private void FixPosOnWayDown()
    {
        if (decreaseHeight == false)
        {
            return;            
        }

        if (startTimer == false)
        {
            startTimer = true;
            timer = Time.timeSinceLevelLoad + timeAdd;
            //timerShutOff = Time.timeSinceLevelLoad + timeAdd + 3;
            
            // Calculate the speed and distance down
            Vector3 distanceInXZ = target.transform.position - this.transform.position;
            distanceInXZ.y = 0;
            float lengthXZ = distanceInXZ.magnitude;
            float timeDown = Mathf.Sqrt(2 * rotationMotor.errorInYWhenNewTarget / -9.81f);

            heightToWidthRatio = lengthXZ / (target.transform.position.y - this.transform.position.y);
            //farten som trengs i XZ på veg ned
            velocityToReach = lengthXZ / timeDown;

            //Finner farten i bare XZ retning

        }

        if (timer < Time.timeSinceLevelLoad && startTimer == true)
        {
            Vector3 rigidbodySpeed = GetComponent<Rigidbody>().velocity;
            rigidbodySpeed.y = 0;
            currentSpeedInXZ = rigidbodySpeed.magnitude;
            executeFallBurn = true;


            if (this.transform.position.y < Mathf.Abs(rotationMotor.errorInYWhenNewTarget) * 0.5f)
            {
                //resetBurn = true;
                if (resetBurn == false)
                {
                    resetBurn = true;
                    timerShutOff = Time.timeSinceLevelLoad + timeAdd + timeAdd;
                }

                if (timerShutOff > Time.timeSinceLevelLoad)
                {
                    motor.RotationUseMotor();
                    usingMotor = true;
                }
                else if (currentSpeedInXZ > velocityToReach)
                {
                    usingMotor = false;
                }


            }

            if (this.GetComponent<Rigidbody>().velocity.y > 0.5f)
            {

                goingDownForGood = true;
                startTimer = false;
                decreaseHeight = false;
            }
            


            

            if (currentSpeedInXZ < velocityToReach)
            {
                usingMotor = true;
                motor.RotationUseMotor();
            }
            else if (timerShutOff < Time.timeSinceLevelLoad)
            {
                usingMotor = false;
            }

            if (this.transform.position.y < Mathf.Abs(rotationMotor.errorInYWhenNewTarget) * 0.1f)
            {
                decreaseHeight = false;
                startTimer = false;
                executeFallBurn = false;
                resetBurn = false;
                goingDownForGood = false;
                
            }

            if (motor.thrust > 0.5f)
            {
                usingMotor = false;
            }
        }

    }
}
