using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathScript : MonoBehaviour {

    Vector3 newPos, oldPos;
    float newTime, oldTime;

    float newErrorLeft, oldErrorLeft;

    private UnityController myUC;
    private Movement myM;
    public float totalError = 0f;


	// Use this for initialization
	void Start () {
        myUC = this.GetComponent<UnityController>();
        myM = this.GetComponent<Movement>();

        newPos = this.transform.position;
        newTime = Time.time;
	}

    public float Derivative()
    {

        newErrorLeft = AmountOfError();
        totalError += AmountOfError();

        float currentErrorSpeed;


        currentErrorSpeed = (newErrorLeft - oldErrorLeft) / Time.deltaTime;

        oldErrorLeft = newErrorLeft;

        return currentErrorSpeed;
    }

    private float AmountOfError()
    {

        Vector3 errorInVector = myM.target.transform.position - this.transform.position;
        float errorLeft = errorInVector.magnitude;

        return errorLeft;
    }




}
