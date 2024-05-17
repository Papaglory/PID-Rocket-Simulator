using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorVinkel : MonoBehaviour {

    float oldErrorLeft, newErrorLeft;
    Movement myM;

	// Use this for initialization
	void Start () {
        myM = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //DENNE GIR FARTEN SOM ERROR FORSVINNER MED
    public float Derivative()
    {


        oldErrorLeft = newErrorLeft;
        newErrorLeft = AmountOfError(); //DETTE ER DEN NYE ERROREN
        /*
        oldTime = newTime;
        newTime = Time.time;
        */
        float currentErrorSpeed;

        //MULIGENS DET ER FEIL Å BRUKE TIME.DELTATIME

        currentErrorSpeed = (newErrorLeft - oldErrorLeft) / Time.deltaTime;

        return currentErrorSpeed;
    }

    private float AmountOfError()
    {

        Vector3 errorInVector = myM.target.transform.position - this.transform.position;
        float errorLeft = errorInVector.magnitude;

        return errorLeft;
    }
}
