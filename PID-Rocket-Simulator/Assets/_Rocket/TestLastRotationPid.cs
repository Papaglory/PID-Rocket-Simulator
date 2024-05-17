using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLastRotationPid : MonoBehaviour
{
    [Header("PID constant")]
    //ANBEFALT: Kp = 0.6, Kd = 4, Ki = 0.0679258
    [SerializeField] float Kp, Kd, Ki;
    [SerializeField] Transform target;
    [Tooltip("Dette kan me se på som farten til rotasjonen")]
    [SerializeField] float min = -100, max = 100;
    //ANBEFALT distanceToActivateIntegral = 20
    [SerializeField] float distanceToActivateIntegral;

    public float errorWhenNewTarget;
    float integral = 0, oldError = 0, output, percentageOutput, oldErrorSlope;

    //private void Start() { errorWhenNewTarget = Mathf.Abs((target.position.z - this.transform.position.z)); }

    public void PidCalculation(Vector3 target, Vector3 currentValue)
    {
        Vector3 error = target - currentValue;
        float fError = error.z;

        //Reset integral verdi viss avstand blir for stor, eller viss error=0
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
    public float GetPercentageOutput() { return percentageOutput; }
}
