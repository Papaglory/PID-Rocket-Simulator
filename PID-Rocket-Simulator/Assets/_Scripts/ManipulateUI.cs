using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulateUI : MonoBehaviour {

    [SerializeField] GameObject[] allButtons;
    bool activated = false;


    public void ButtonDown()
    {
        activated = !activated;
        for (int i = 0; i<allButtons.Length; i++)
        {
            allButtons[i].SetActive(!activated);
        }
    }
}
