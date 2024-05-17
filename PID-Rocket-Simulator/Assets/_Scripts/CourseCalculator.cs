using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseCalculator : MonoBehaviour {


    [SerializeField] Transform target;

    Vector3 VectorMeToTarget;
    float degree;
    float xValue, yValue, zValue;
    float error;

    // Use this for initialization
    void Start () {
        //DETTE ENDRER ROTASJONEN TIL OBJEKTET
        //this.transform.rotation = Quaternion.Euler(20f, 0, 0);
        
	}
	
	// Update is called once per frame
	void Update () {
        TargetCourse();
        DegreesTotarget();
        
        
        //DETTE ROTERER OBJEKTET MED 1 GRAD I SEKUNDET
        //this.transform.Rotate(Vector3.right * Time.deltaTime);
       
	}

    //Positiv retning opp og til venstre
    public float DegreesTotarget()
    {
        zValue = VectorMeToTarget.z;
        yValue = VectorMeToTarget.y;

        degree = Mathf.Atan2(yValue,zValue)*180 /3.14f;
        error = 90 - this.transform.eulerAngles.x - degree;

        return error;

        
    }

    private void TargetCourse()
    {
        //DETTE FINNER RETNING I FRÅ RAKETT TIL TARGET OG GJØR SLIK AT KVAR VERDI I KVAR ENKELT DIMENSJON ER MELLOM 1 OG 0
        VectorMeToTarget = target.transform.position - this.transform.position;
        VectorMeToTarget = VectorMeToTarget.normalized;


    }

    public float TargetCourseReal()
    {
        float length;
        VectorMeToTarget = target.transform.position - this.transform.position;
        length = VectorMeToTarget.magnitude;
        return length;
    }

    //DENNE SKAL FINNE UT KVA FARTSVEKTOR SOM TRENGS FOR Å KOMME SEG TIL GITT PUNKT

    /*
     * Eg kan prøve å rekne ut den potensielle energien som trengs for å nå målet. Då tar me target.position.y - this.posision.y
     * Det er fullt mogleg å lage ein suicide burn som bremser opp i x-retning
     *      -Dette fører til at ein kan få stor fart i begynnelsen og seinare ta å bremse den ned
     *      */

    //SLIK FINNER EG UT KORLEIS EG FÅR FARTSVEKTOR TIL Å VERE LIK TARGETCOURSE VECTOR
    /* 
     * Viss høgde er rett så er fartsvektor slik at det er nok fart i y-retning til å holde samme høgde og resten går til fart i x-retning
     * Lage ein suicide burn til sidene for å måle om farten i x-retning er stor nok til at den må bli bremset ned
     * 
     * */

    

}
