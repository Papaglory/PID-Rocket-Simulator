using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

    public GameObject myParticle;
    private Movement myMovement;
    float totalDistance;
    float distanceLeft;
    float distancePassed;
    float speedAtCutOff;
    float myAcceleration;
    float timeUntilTop;
    float isThisTopCheck;
    float isThisTopCheckUnity;
    float currentSpeed;

    float timeUntilTopUnity;

    float distanceAtCutOff;

    
    float isDistanceCorrect;

    bool usedMotor;

    // Use this for initialization
    void Start () {
        myMovement = GetComponent<Movement>();
        myAcceleration = myMovement.thrust;

        
    }
	
	// Update is called once per frame
	void Update () {


        print(currentSpeed);
    }

    private void FixedUpdate()
    {

        if (!CutOff())
        {
            myMovement.UseMotor();
            usedMotor = true;
        }
        else if (CutOff())
        {
            usedMotor = false;
        }

    }

    private bool CutOff()
    {
        //Viss motor brenner. rekn ut fart med akselerasjon oppover. viss motor ikkje brenner, rekn ut med akselerasjon nedover. 
        if (usedMotor)
        {
            currentSpeed = (Mathf.Sqrt(2 * (myAcceleration - 1) * 9.81f * this.transform.position.y));
        }
        else if (!usedMotor)
        {
            currentSpeed = (Mathf.Sqrt(2 * (myAcceleration - 1) * 9.81f * this.transform.position.y)) /(9.81f);
        }
        
        distanceLeft = myMovement.target.position.y - this.transform.position.y;

        //Sjekker om eg skal bruke formel for å komme meg opp til target eller om eg skal komme meg ned til target og bremse opp
        if (distanceLeft<0)
        {
            //Her er target under oss siden negativ
        }
        else if (distanceLeft>0) {

            //BRUK AV MATEMATIKK
            timeUntilTop = (Mathf.Sqrt(2 * (myAcceleration - 1) * 9.81f * this.transform.position.y)) / (9.81f);


            //MATEMATIKK
            isThisTopCheck = (Mathf.Sqrt(2 * (myAcceleration - 1) * 9.81f * (this.transform.position.y)) * timeUntilTop) + (0.5f * -9.81f * timeUntilTop * timeUntilTop);
            //Her er target over oss siden positiv
        }
        //speedAtCutOff = Mathf.Sqrt(2 * myAcceleration *9.81f* (distanceLeft)); //Farten ved CutOff



        //BRUKER RIGIDBODY FOR Å FINNE FART
        //timeUntilTopUnity = myMovement.myRigidbody.velocity.y / 9.81f;

        //her får me timeUntilTop som viser kor lenge me kommer til å seile viss me kutter av motoren ved denne høyden
        //når me putter inn tiden i formelen isThisTopCheck så finner me ut kor høgt me kommer når me flyter med tiden i timeUntilTop. Da får me maks høgde når me har ein gitt starfart og avstandigjen


        //RIGIDBODY
        //isThisTopCheckUnity = myMovement.myRigidbody.velocity.y * timeUntilTop + 0.5f * -9.81f * timeUntilTop * timeUntilTop;

        //Eg sjekker først om distanceLeft er positiv eller negativ. er den positiv fortsetter eg, er den negativ må eg ta absolutt verdi og samtidig sjå til at raketten går nedover uten å brenne. 
        //Må lage ein formel som bremser opp dersom rakett er over målet slik at den bremser perfekt som suicide burn
        //Sjekker om ein når bakken (target viss den er under raketten) når ein har ein drop-fart v_0 på veg ned. 


        //Her så ser me om avstanden me får i isThisTopCheck er meir eller mindre enn avstanden til målet. Er den mindre returnerer me false for å kjøre vidare. er den høgare er me forbi målet
        //og me vil da kutte motoren og bytte over til den andre funksjonen som tar på motoren igjen for å treffe suicide burn sweet spot-en





        if (isThisTopCheck > distanceLeft)
        {
            myParticle.SetActive(false);
            return true;
        }

        myParticle.SetActive(true);
        return false;

        

    }
    

    
}
