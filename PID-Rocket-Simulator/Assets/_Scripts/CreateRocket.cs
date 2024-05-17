using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRocket : MonoBehaviour {

    [SerializeField] Text dropdownText;

    string rocketSelect;
    string rocketVacuum = "PidRocket Vacuum", rocketAir = "PidRocket Air", rocketPhysics = "Rocket Physics";
    int rocketID;

    [SerializeField] GameObject[] rockets;
    [SerializeField] Toggle xToggle, zToggle;
	
	// Update is called once per frame
	void Update () {


        if (dropdownText.text == rocketVacuum)
        {
            //ROCKETVACUUM
            rocketID = 0;            
            
        }
        else if (dropdownText.text == rocketAir)
        {
            //ROCKETAIR
            rocketID = 1;            
        }
        else 
        {
            //FYSIKKRAKETT
            rocketID = 2;            
        }		

	}

    public void InstantiateRocket()
    {
        GameObject rocketClone = (GameObject)Instantiate(rockets[rocketID], rockets[rocketID].transform.position, rockets[rocketID].transform.rotation);
        

        if (rocketID == 2)
        {
            return;
        }
        if (!xToggle.isOn)
        {
            if (rocketID == 0)
            {
                rocketClone.GetComponent<TestRotationMotor>().enabled = false;
                rocketClone.GetComponent<TestRotationPid>().enabled = false;
            }
            else
            {
                rocketClone.GetComponent<RotationPid>().enabled = false;
                rocketClone.GetComponent<RotationMotor>().enabled = false;
            }

        }

        if (!zToggle.isOn)
        {
            if (rocketID == 0)
            {
                rocketClone.GetComponent<TestLastRotationMotor>().enabled = false;
                rocketClone.GetComponent<TestLastRotationPid>().enabled = false;
            }
            else
            {
                rocketClone.GetComponent<LastRotationPid>().enabled = false;
                rocketClone.GetComponent<LastRotationMotor>().enabled = false;
            }
        }



        //VISS TOGGLETRUE
        //SKRU AV SCRIPTS
        //ELSE INGENTING
    }
}
