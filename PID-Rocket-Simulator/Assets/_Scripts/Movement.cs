using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Transform target;
    [Tooltip("Kraft som går i retning this.transform.up, der kvar eining er 9.81m/s^2. Hovering ved thrust=1")]
    public float thrust = 0;
    public Rigidbody myRigidbody;

    //[SerializeField] float sideThrust = 2;
    [SerializeField] float proportional = 1;
    [SerializeField] float derivative = 1;
    [SerializeField] float integral = 1;


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    //HØGRE ER POSITIV RETNING; VENSTRE ER NEGATIV
    //VISS INT COURSE ER POSITIV ER DET MOT HØGRE, ELLERS ER DET MOT VENSTRE VISS DEN ER NEGATIV
    public void ChangeCourse(int course, float currentError, float errorSpeed, float totalError)
    {
        //float output = (currentError * proportional) + (errorSpeed * derivative) + (totalError * integral);
        //myRigidbody.AddTorque(this.transform.right * course * ((currentError*proportional) + errorSpeed * derivative), ForceMode.Acceleration);
        //this.transform.Rotate((Vector3.right * sideThrust * course * Time.deltaTime) * currentError / proportional);
    }

    

    public void UseMotor()
    {
        myRigidbody.AddForce(this.transform.up *  9.81f *thrust, ForceMode.Acceleration);
        //print(this.transform.position.y);
    }


}
