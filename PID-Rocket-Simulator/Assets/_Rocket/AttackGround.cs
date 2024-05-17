using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGround : MonoBehaviour {


    [SerializeField] Transform Target;
    Vector3 wayToTarget;
    Vector3 velocity;


    private void Update()
    {
        if (this.gameObject.GetComponent<Rigidbody>().velocity == null)
        {
            //IKKJE FERDI ENDA
            return;
        }

        wayToTarget = (Target.position - this.transform.position).normalized;
        velocity = this.gameObject.GetComponent<Rigidbody>().velocity.normalized;

        if (velocity.x > wayToTarget.x)
        {

        } 
        else if (velocity.x < wayToTarget.x)
        {

        }

        float errorX = wayToTarget.x - velocity.x;



        //VELOCITY VECTOR SKAL VERE PARALELL MED WAYTOTARGET VECTOR
    }


}
