using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePosUI : MonoBehaviour {

    float xValue, yValue, zValue;
    [SerializeField] GameObject[] coordinateValues;
    [SerializeField] GameObject target;
    [SerializeField] GameObject myRedTarget;
    [SerializeField] Toggle myToggle;

    private void Start() {
        OnButtonDown();
    }

    private void Update()
    {
        if (myToggle.isOn)
        {
            OnButtonDown();
        }
    }

    public void OnButtonDown()
    {
        xValue = coordinateValues[0].GetComponent<Slider>().value;
        yValue = coordinateValues[1].GetComponent<Slider>().value;
        zValue = coordinateValues[2].GetComponent<Slider>().value;
        if (yValue < 0)
        {
            yValue = 0;
        }
        
        Vector3 newPos = new Vector3(xValue, yValue, zValue);
        target.transform.position = newPos;
        myRedTarget.transform.position = newPos;
    }
}
