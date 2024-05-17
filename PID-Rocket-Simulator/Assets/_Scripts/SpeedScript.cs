using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody myRigidbody = this.gameObject.GetComponent<Rigidbody>();

        myRigidbody.AddForce(Vector3.up * 62.6958390584633011f, ForceMode.VelocityChange);


    }
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y >= 200)
        {
            //print("WE MADE IT ");
        }
	}
}
