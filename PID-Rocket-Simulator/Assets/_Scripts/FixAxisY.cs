using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAxisY : MonoBehaviour {

    float localDegrees;
	
	
    
    // For some reason, fixing rotation y in the rigidbody is not sufficient
	void FixedUpdate () {
        localDegrees = this.transform.localEulerAngles.y;
        localDegrees = (localDegrees > 180) ? localDegrees - 360 : localDegrees;

        if (localDegrees != 0)
        {
           
            Quaternion originalRot = this.transform.rotation;
            this.transform.rotation = Quaternion.Euler(originalRot.eulerAngles.x, 0, originalRot.eulerAngles.z);
        }

	}
}
