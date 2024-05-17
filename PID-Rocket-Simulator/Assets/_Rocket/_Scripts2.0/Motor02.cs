using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor02 : MonoBehaviour
{

    Rigidbody myRigidbody;
    PidController02 myP;
    AdjustFallingPath fallingPath;
    [SerializeField] Transform target;
    public float thrustRate;
    public float max;
    float motorPercentageUsed = 0;
    bool myMotors = false;

    [Header("Total amount of thrust used from start")]
    [SerializeField] float totalFuel;
    [SerializeField] bool resetFuel = false;

    [SerializeField] GameObject particleFire;
    [SerializeField] GameObject particleSmoke;
    // Here are recommended values: THRUST = 0, THRUST RATE = 60, MIN = 0, MAX = 30
    // MASS = 1
    public float thrust;

    Vector3 currentTargetPos;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myP = GetComponent<PidController02>();
        fallingPath = GetComponent<AdjustFallingPath>();
        currentTargetPos = target.transform.position;
    }

    private void Update()
    {
        NewTargetPosition();
        ParticleSystem();
        ResetFuel();
        motorPercentageUsed = thrust / max;

    }


    void FixedUpdate()
    {

        myP.PidCalculation(target.position, this.transform.position, myRigidbody.velocity.y);

        ChangeThrust();
        UseMotor(myP.GetOutput());
    }

    private void ResetFuel()
    {
        if (resetFuel == true)
        {
            resetFuel = false;
            totalFuel = 0;
        }
    }

    // This is to land at the target on the way down in the XZ-plane
    public void RotationUseMotor()
    {
        totalFuel += 11 * Time.fixedDeltaTime;
        myRigidbody.AddForce(this.transform.up * 11, ForceMode.Acceleration);
    }

    private void UseMotor(float output)
    {
        if (float.IsNaN(thrust) || float.IsNaN(output))
        {
            return;
        }
        totalFuel += thrust * Time.fixedDeltaTime;
        myRigidbody.AddForce(this.transform.up * thrust * Mathf.Clamp01(output), ForceMode.Acceleration);
    }



    // This method changes the speed by comparing currentThrust in percentage with the output from the PID in percentage.
    // Since out
    private void ChangeThrust()
    {
        float percentageThrust = thrust / max;


        if (myP.GetOutput() > percentageThrust)
        {
            thrust += thrustRate * Time.fixedDeltaTime;
        }

        if (myP.GetOutput() == 0)
        {
            thrust = 0;
            return;
        }

        if (myP.GetOutput() < percentageThrust)
        {
            thrust -= thrustRate * Time.fixedDeltaTime;
        }

    }

    private void ParticleSystem()
    {
        if (thrust > 0)
        {
            particleFire.SetActive(true);
            particleSmoke.SetActive(true);
            particleFire.GetComponent<ParticleSystem>().startSpeed = 76 * motorPercentageUsed;
            particleFire.GetComponent<ParticleSystem>().startSize = 8 * motorPercentageUsed;
        }

        else if (fallingPath.usingMotor)
        {
            particleFire.SetActive(true);
            particleSmoke.SetActive(true);
            particleFire.GetComponent<ParticleSystem>().startSpeed = 76 *0.18f;
            particleFire.GetComponent<ParticleSystem>().startSize = 8 * 0.18f;
        }
        else if (thrust <= 0 || fallingPath.usingMotor == false)
        {
            particleFire.SetActive(false);
            particleSmoke.SetActive(false);
            particleFire.GetComponent<ParticleSystem>().startSpeed = 0 * motorPercentageUsed;
            particleFire.GetComponent<ParticleSystem>().startSize = 0 * motorPercentageUsed;
        }

        if (fallingPath.usingMotor)
        {
            particleFire.SetActive(true);
            particleSmoke.SetActive(true);
            particleFire.GetComponent<ParticleSystem>().startSpeed = 76 * 0.18f;
            particleFire.GetComponent<ParticleSystem>().startSize = 8 * 0.18f;
        }

        if (fallingPath.goingDownForGood && thrust<=0)
        {
            particleFire.SetActive(false);
            particleSmoke.SetActive(false);
        }

        if (thrust > 0)
        {
            particleFire.SetActive(true);
            particleSmoke.SetActive(true);
            particleFire.GetComponent<ParticleSystem>().startSpeed = 76 * motorPercentageUsed;
            particleFire.GetComponent<ParticleSystem>().startSize = 8 * motorPercentageUsed;
            
        }
    }


    private void NewTargetPosition()
    {
        if (currentTargetPos != target.transform.position)
        {
            currentTargetPos = target.transform.position;
            myP.newTargetPos = true;
        }
    }

}
