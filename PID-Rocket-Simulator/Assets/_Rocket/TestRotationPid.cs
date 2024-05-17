using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationPid : MonoBehaviour
{
    [Header("PID constant")]
    [SerializeField] float Kp, Kd, Ki;
    [SerializeField] Transform target;
    [Tooltip("Rotational speed")]
    [SerializeField] float min = -1, max = 1;
    [SerializeField] float distanceToActivateIntegral;

    public float errorWhenNewTarget;
    float integral = 0, oldError = 0, output, percentageOutput, oldErrorSlope;

    public void PidCalculation(Vector3 target, Vector3 currentValue)
    {
        Vector3 error = target - currentValue;
        float fError = error.x;

        //Reset integral value if the distance gets too large or if the error = 0
        if (Mathf.Abs(fError) > distanceToActivateIntegral)
        {
            integral = 0;
        }

        if (integral == fError)
        {
            integral = 0;
        }

        integral += fError * Time.fixedDeltaTime;

        float errorSlope = fError - oldError;
        errorSlope = errorSlope / Time.fixedDeltaTime;

        

        output = (integral * Ki) + (errorSlope * Kd) + (fError * Kp);

        percentageOutput = Mathf.Clamp(output, min, max);
        percentageOutput = percentageOutput / max;

        output = Mathf.Clamp(output, min, max);

        oldError = fError;
        oldErrorSlope = errorSlope;
    }

    public float GetOutput() { return output; }
    public float GetPercentageOutput() { return -percentageOutput; }
}
