using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTarget : MonoBehaviour {

    [SerializeField] GameObject myRedTarget;
    bool activated = false;

    public void OnThisButtonDown()
    {
        activated = !activated;
        myRedTarget.GetComponent<MeshRenderer>().enabled = activated;
    }
}
