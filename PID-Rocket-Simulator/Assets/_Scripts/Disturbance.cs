using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disturbance : MonoBehaviour {

    [SerializeField] float power;


	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
        {

            this.transform.Rotate(this.transform.up * power * Time.deltaTime);
        }
	}
}
