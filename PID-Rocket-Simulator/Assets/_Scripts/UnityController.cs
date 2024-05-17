using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityController : MonoBehaviour
{

    public GameObject myParticleOne;
    public GameObject myParticleTwo;
    private Movement myMovement;
    private CourseCalculator myCourseCalculator;
    private MathScript myMathScript;

    float totalDistance;
    float distanceLeft;
    float distancePassed;
    float speedAtCutOff;
    float timeUntilTop;
    float isThisTopCheck;
    float isThisTopCheckUnity;
    float currentSpeed;
    float timeUntilTopUnity;
    float distanceAtCutOff;
    float isDistanceCorrect;

    float timer;

    //FLOATS FOR SUICIDEBURN METHOD
    float suicideBurnTimeUntilTarget;
    float suicideBurnVelocity;
    float suicideBurnIsThisTarget;

    float error;

    float currentLength;

    public bool rocketOn = false;

    //NOTAT TIL ENGINE

        /*
         * Me endrer thrust valuen i frå 0 til max som current er 3. Endringen vil vere thrust rate og den vil endre seg i sekund med time.deltatime
         * 
         * 
         * 
         * */
    [SerializeField ]float maxThrust = 3;

    [SerializeField] float thrustRate = 6;

    void ChangeThrust()
    {
        if (rocketOn)
        {
            if (myMovement.thrust > maxThrust)
            {
                myMovement.thrust = maxThrust;
            }

            if (myMovement.thrust < maxThrust)
            {
                myMovement.thrust += thrustRate * Time.deltaTime;
                //ENDRE THRUST
                //MULIGENS BRUKE TIME.FIXEDDELTATIME

                //AUKER PARTICLEEFFECT MED THRUST SOM VALUE. MULIGENS TA THRUST/X;
            }
        }
        if (!rocketOn)
        {
            if (myMovement.thrust > 0)
            {
                myMovement.thrust -= thrustRate * Time.deltaTime;
                //MULIGENS BRUKE TIME.FIXEDDELTATIME
                //MINKE PARTICLEEFFECT MED THRUST SOM VALUE. MULIGENS TA THRUST/X;
            }
        }
    }





    void Start()
    {
        myMovement = GetComponent<Movement>();
        myCourseCalculator = GetComponent<CourseCalculator>();
        myMathScript = GetComponent<MathScript>();



    }
    void Update()
    {


        ChangeThrust();


        if (myMovement.thrust > 0)
        {
            rocketOn = true;
        }
        if (myMovement.thrust < 0)
        {
            rocketOn = false;
        }


        if (myMovement.thrust <0)
        {
            myParticleOne.SetActive(false);
            myParticleTwo.SetActive(false);
        }
        if (myMovement.thrust > 0)
        {
            myParticleOne.SetActive(true);
            myParticleTwo.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        if(this.transform.position.y < myMovement.target.position.y)
        {
            BurnForward();
        }
        if (this.transform.position.y > myMovement.target.position.y)
        {
            SuicideBurn();
        }

        //DETTE ER KODEN SOM ROTERER
        error = myCourseCalculator.DegreesTotarget();
        if (error > 0)
        {
            currentLength = myCourseCalculator.TargetCourseReal();
            myMovement.ChangeCourse(1, currentLength, myMathScript.Derivative(), myMathScript.totalError);
            //ROTER med minus sideburnthrottle
        }

        if (error < 0)
        {
            currentLength = myCourseCalculator.TargetCourseReal();
            myMovement.ChangeCourse(-1, currentLength, myMathScript.Derivative(), myMathScript.totalError);
            //ROTER MED POSITIV SIDEBURNTHROTTLE
        }
    }

    private void SuicideBurn()
    {

        /* Me ser på farten i det punktet den detter ned ved kvar update
         * Me ser om raketten kan bremse opp når den brenner ved å motvirke gravitasjonskreftene (DET BLIR SLIK: thrust*9.81 (9.81 er kraften i raketten per eining) -9.81m/s^2 som er gravitasjonskraften
         * Viss me er over punktet og me er innenfor slik at v_1(sluttfarten) blir 0 ved target sin height så skal den skru på raketten.
         * 
         */



        suicideBurnVelocity = myMovement.myRigidbody.velocity.y;

        //aktiver suicideburn method,, denne skal mulignes bli plassert i fixedupdate method


        suicideBurnTimeUntilTarget = Mathf.Abs(suicideBurnVelocity / (2 * 9.81f));
        suicideBurnIsThisTarget = suicideBurnVelocity * suicideBurnTimeUntilTarget + 0.5f * (2 * 9.81f) * suicideBurnTimeUntilTarget * suicideBurnTimeUntilTarget;
        //suicideBurnIsThisTarget = suicideBurnIsThisTarget - myMovement.target.transform.position.y;
        

        suicideBurnIsThisTarget = this.transform.position.y - Mathf.Abs(suicideBurnIsThisTarget);

        

        if (suicideBurnIsThisTarget < myMovement.target.transform.position.y && suicideBurnVelocity < 0)
        {
           rocketOn = true;
           myMovement.UseMotor();
            return;
        }

        rocketOn = false;
    }

    private void BurnForward()
    {
        distanceLeft = myMovement.target.transform.position.y - this.transform.position.y;

        //Sjekker om eg skal bruke formel for å komme meg opp til target eller om eg skal komme meg ned til target og bremse opp
        if (distanceLeft < 0)
        {
            //Her er target under oss siden negativ
        }
        else if (distanceLeft > 0)
        {

            //Her er target over oss siden positiv
        }

        

        //BRUKER RIGIDBODY FOR Å FINNE FART
        timeUntilTopUnity = myMovement.myRigidbody.velocity.y / 9.81f;

        //her får me timeUntilTop som viser kor lenge me kommer til å seile viss me kutter av motoren ved denne høyden
        //når me putter inn tiden i formelen isThisTopCheck så finner me ut kor høgt me kommer når me flyter med tiden i timeUntilTop. Da får me maks høgde når me har ein gitt starfart og avstandigjen


        //RIGIDBODY
        isThisTopCheckUnity = myMovement.myRigidbody.velocity.y * timeUntilTopUnity + 0.5f * -9.81f * timeUntilTopUnity * timeUntilTopUnity;

        //Eg sjekker først om distanceLeft er positiv eller negativ. er den positiv fortsetter eg, er den negativ må eg ta absolutt verdi og samtidig sjå til at raketten går nedover uten å brenne. 
        //Må lage ein formel som bremser opp dersom rakett er over målet slik at den bremser perfekt som suicide burn
        //Sjekker om ein når bakken (target viss den er under raketten) når ein har ein drop-fart v_0 på veg ned. 


        //Her så ser me om avstanden me får i isThisTopCheck er meir eller mindre enn avstanden til målet. Er den mindre returnerer me false for å kjøre vidare. er den høgare er me forbi målet
        //og me vil da kutte motoren og bytte over til den andre funksjonen som tar på motoren igjen for å treffe suicide burn sweet spot-en



        //Viss raketten er over target sin height, så skal den andre formelen slå til slik at det blir mulig å utføre suicide burn

        //Her skrur me av motoren
        //Denne koden blir brukt når target er under raketten


        //Her tar me av motoren
        //Denne koden blir brukt for å seile til toppen
        //Blir brukt når me er under target, men skal ikkje bruke raketten
        if (isThisTopCheckUnity > distanceLeft && myMovement.myRigidbody.velocity.y >0)
        {
            rocketOn = false;
             return;
        }


        //Denne blir brukt for å komme seg oppover

        rocketOn = true;
        myMovement.UseMotor();

        if (distanceLeft < 0)
        {
            rocketOn = false;
            return;
        }
    }
}
