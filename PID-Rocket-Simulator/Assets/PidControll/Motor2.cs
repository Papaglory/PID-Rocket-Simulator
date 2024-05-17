using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor2 : MonoBehaviour {

    Rigidbody myRigidbody;
    PidController myP;
    [SerializeField] Transform target;
    public float thrustRate;
    public float sailingThrust;
    public float max;

    [Header("Total amount of thrust used from start")]
    [SerializeField] float totalFuel;
    [SerializeField] bool resetFuel = false;

    [SerializeField] GameObject particleFire;
    [SerializeField] GameObject particleSmoke;
    //HER ER ANBEFALT VERDIER: THRUST = 0, THRUST RATE = 60, MIN = 0, MAX = 30
    //MASS = 1
    public float thrust;

    Vector3 currentTargetPos;


    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        myP = GetComponent<PidController>();
        currentTargetPos = target.transform.position;
    }

    private void Update()
    {
        NewTargetPosition();
        ParticleSystem();
        ResetFuel();
    }




    void FixedUpdate() {
        
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


    private void UseMotor(float output)
    {
        if (float.IsNaN(thrust) || float.IsNaN(output))
        {
            return;
        }
        totalFuel += thrust * Time.fixedDeltaTime;
        myRigidbody.AddForce(this.transform.up * thrust * Mathf.Clamp01(output), ForceMode.Acceleration);
    }

    public void MotorForSide()
    {
        totalFuel += sailingThrust * Time.fixedDeltaTime;
        myRigidbody.AddForce(this.transform.up * sailingThrust, ForceMode.Acceleration);
    }


    //Denne metoden endrer farten ved å sammenlikne currentThrust i prosent med output fra PID sin prosent.
    //Siden out
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
        if (thrust > 0 || sailingThrust > 0)
        {
            particleFire.SetActive(true);
            particleSmoke.SetActive(true);
        }
        else if (thrust <= 0 || sailingThrust <= 0)
        {
            particleFire.SetActive(false);
            particleSmoke.SetActive(false);
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
