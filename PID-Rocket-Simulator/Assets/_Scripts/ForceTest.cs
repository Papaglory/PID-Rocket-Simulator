using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour {


    Rigidbody myR;
	// Use this for initialization
	void Start () {
        myR = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            Push();
        }
	}

    private void Push()
    {
        print("PUSHED");
        Vector3 thisPoint = this.transform.position + new Vector3(0, 1, 0);
        myR.AddForceAtPosition(this.transform.forward * 2f, thisPoint);
    }
}
