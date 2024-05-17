using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour {

    Rigidbody myR;

	// Use this for initialization
	void Start () {
        myR = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        myR.velocity = new Vector3(0, 30, 50);
		
	}
}
