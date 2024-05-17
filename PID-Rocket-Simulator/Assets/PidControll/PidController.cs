using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidController : MonoBehaviour {


    [Header("PID constant")]
    [SerializeField] float Kp, Kd, Ki; //Kp = 0.76 og Kd = 0.44 ser ut til å funke med gravitasjon og masse = 1 
    //Ki på 0.2 ser ut til å funke greit med 0.6 drag i rigidbody
    [SerializeField] bool isClamp;
    [Tooltip("Clamp der min = 0 og max = 1 er default. En vil da få prosenten")]
    public float min, max;
    //ANBEFALTE VERDIER: ISCLAMP = TRUE, MIN = 0, MAX = 1

    [SerializeField] Transform target;

    float integral = 0, oldError = 0, output;
    
    float bodyHeight = 1 + 0.5f;

    public bool newTargetPos;
    public float errorWhenNewTarget;

    

    private void Start()
    {
        errorWhenNewTarget = Mathf.Abs((target.position.y - this.transform.position.y));
        
    }

    



    public void PidCalculation(Vector3 target, Vector3 currentValue, float currentVelocity)
    {
        //target.y = target.y - bodyHeight; //DENNE KODEN OVER ER BARE FOR AT HEISEN SKAL STOPPE MED BEINA TIL MANNEN OG IKKJE INNE I HAN    


        Vector3 error = target - currentValue;
        float fError = error.y;
        float constant;




        float fErrorForConstant = 0;
        float absError = Mathf.Abs(fError);



        //VISS DEN ER OVER TARGET SÅ MÅ FERROR VERE POSITIV VISS DEN ER UNDER MÅ DEN VERE NEGATIV
        //TRENGER DERFOR DA MEST SANSYNLIG TO FORSKJELLIGE

        //DEN KAN IKKJE ENDRE SEG HEILE TIDEN. DEN KAN KUN ENDRE SEG EIN GANG NÅR DEN SKAL BEGYNNE Å FALLE
        //VISS DEN ENDRER SEG HEILE TIDEN SÅ "GLEMMER DEN UT" KOR DEN BEGYNTE FOR VERDIEN VIL HEILE TIDEN BLI ARSTATTET
        //FIX: PRØVE Å LAGE EIN FLOAT SOM TAR HEIGHTVALUEATNEWTARGET SOM ER DEN SOM BLIR BRUKT FRAM TIL ME HAR KOMT FRAM TIL TARGET
        float sqrtConstantUp, sqrtConstantDown;

        
        if (newTargetPos)
        {
            errorWhenNewTarget = absError;
            
            newTargetPos = false;
            
        }

        if (errorWhenNewTarget < 1000)
        {
            errorWhenNewTarget = 1000;
        }



        if (fError <= 0)
        {

            //NEGATIV
            

            sqrtConstantDown = (400000f - Mathf.Sqrt((400000f * 400000f) - 4f * 400000f * (101000f - errorWhenNewTarget))) / (2f * 400000f);
            
            fErrorForConstant = -Mathf.Pow(absError, sqrtConstantDown);
            constant = Mathf.Abs(currentVelocity / fErrorForConstant);
        } else
        {
            //POSITIV
            

            sqrtConstantUp = (400000f - Mathf.Sqrt((400000f * 400000f) -4f * 400000f * (101000f - errorWhenNewTarget))) / (2f * 400000f);
            fErrorForConstant = Mathf.Pow(fError, sqrtConstantUp * 0.75f);

            constant = Mathf.Abs(currentVelocity / fErrorForConstant);  // DETTE ER FOR Å FÅ EIN KONSTANT SOM BREMSER OPP TIL NÆRMARE ME KOMME
        }


        



        integral += fError * Time.fixedDeltaTime;

        float errorSlope = fError - oldError;

        errorSlope = errorSlope / Time.fixedDeltaTime;

        output = (integral * Ki) + (errorSlope * Kd * constant) + (fError * Kp);

        //Clamp mellom 0 og 1 er default
        if (isClamp)
        {
            output = Mathf.Clamp(output, min, max);
        }

        oldError = fError;
    }

    public void EnableClamp(float newMin, float newMax)
    {
        isClamp = true;
        min = newMin;
        max = newMax;
    }

    public float GetOutput() { return output; }

    public void DisableClamp() { isClamp = false; }



}
