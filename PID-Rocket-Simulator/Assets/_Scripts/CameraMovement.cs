using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

    [SerializeField] Toggle myToggle;
    [SerializeField] float timeAdd = 5;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] GameObject target;
    [SerializeField] Text myText;
    [SerializeField] Slider cameraDistanaceSlider;
    bool autoMove = false;
    bool timerActivate = false;
    bool isMoving = false;
    int currentPos = 0;
    float timer;
    Vector3 changedPos;
    Vector3 savedPos;
    GameObject followingThisRocket = null;

    private void LateUpdate()
    {
        if (followingThisRocket == null)
        {
            followingThisRocket = GameObject.FindGameObjectWithTag("Player");
        }
        if (followingThisRocket != null)
        {
            myText.text = "Speed: " + followingThisRocket.GetComponent<Rigidbody>().velocity.magnitude + "\n" + "Altitude: " + followingThisRocket.transform.position.y;

        }
        
        //print(currentPos);
        if (myToggle.isOn)
        {
            
            if (currentPos >= 7)
            {
                currentPos = 0;
            }


            if (timerActivate == false)
            {
                timer = Time.timeSinceLevelLoad + 3 + Random.Range(2, 6);
                timerActivate = true;
            }
            if (Time.timeSinceLevelLoad > timer)
            {
                timerActivate = false;
                isMoving = false;
                savedPos = Vector3.zero;
                currentPos++;

            }
            
            switch (currentPos)
            {
                case 0:
                    Vector3 forThisCase = followingThisRocket.transform.position + new Vector3(0.443f, 1.406f, -0.486f);
                    this.transform.position = forThisCase;
                    this.transform.eulerAngles = followingThisRocket.transform.eulerAngles + new Vector3(-80.577f, -46.112f, 0);
                      
                    break;
                    


                case 1:

      
                    if (isMoving == false)
                    {                       
                        changedPos = new Vector3(0, movementSpeed * Time.deltaTime, 0);
                        isMoving = true;
                        
                    }
                    savedPos += changedPos;

                    this.transform.position = followingThisRocket.transform.position + new Vector3(0.55f, 2.28f, -0.59f) + savedPos;
                    this.transform.eulerAngles = followingThisRocket.transform.eulerAngles + new Vector3(80.577f, -46.112f, 0);

                    break;

                case 2:
                    if (isMoving == false)
                    {
                        changedPos = new Vector3(0, -movementSpeed * Time.deltaTime, 0);
                        isMoving = true;


                        
                    }
                    savedPos += changedPos;

                    this.transform.position = followingThisRocket.transform.position + new Vector3(-0.88f, 8.63f, -2f) + savedPos;
                    this.transform.eulerAngles = followingThisRocket.transform.eulerAngles;

                    break;

                case 3:

                    this.transform.position = followingThisRocket.transform.position + new Vector3(0, 10.89f, 0.94f);
                    this.transform.eulerAngles = followingThisRocket.transform.eulerAngles + new Vector3(90, 0, 0);
                   
                    break;

                case 4:

                    this.transform.position = followingThisRocket.transform.position +  new Vector3(0, 10.12f, -29.17f);
                    this.transform.eulerAngles = followingThisRocket.transform.eulerAngles + new Vector3(8, 0, 0);


                    break;

                case 5:

                    if (isMoving == false)
                    {
                        if (target.transform.position.y - followingThisRocket.transform.position.y > 0)
                        {
                            this.transform.position = followingThisRocket.transform.position + new Vector3(27.83f, 50f, 0);
                        }
                        else
                        {
                            if (followingThisRocket.transform.position.y - 22.17f < 0)
                            {
                                this.transform.position = new Vector3(27.83f, 2, 0);
                            }
                            this.transform.position = followingThisRocket.transform.position - new Vector3(27.83f, 50f, 0);
                            
                        }
                        this.transform.eulerAngles = followingThisRocket.transform.eulerAngles + new Vector3(20, -90, 0);
                        isMoving = true;
                    }

                    this.transform.LookAt(followingThisRocket.transform);

                    break;

                case 6:

                    if (isMoving == false)
                    {
                        changedPos = new Vector3(0, 4*movementSpeed * Time.deltaTime, 0);
                        isMoving = true;

                    }
                    savedPos += changedPos;

                    this.transform.position = followingThisRocket.transform.position + new Vector3(3.64f, -27.81f, 3.28f) + savedPos;
                    this.transform.eulerAngles = followingThisRocket.transform.eulerAngles + new Vector3(-90, -0, -50);

                    break;
            }
            
        }         
        else if (followingThisRocket == null)
        {
            this.transform.position = new Vector3(27.83f, 22.17f, 0);
            this.transform.eulerAngles = new Vector3(10, -90, 0);
        } else
        {
            this.transform.position = followingThisRocket.transform.position + new Vector3(27.83f * cameraDistanaceSlider.value + 20, 14, 0);       
            this.transform.eulerAngles = new Vector3(10, -90, 0);
        }
    }
                
    public void OnButtonDown()
    {
        if (myToggle.isOn == false)
        {
            return;
        }
        timerActivate = false;
        isMoving = false;
        savedPos = Vector3.zero;
        currentPos++;
    }

}
