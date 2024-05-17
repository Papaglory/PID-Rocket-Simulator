using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {



    public void RestartThisScene()
    {
        SceneManager.LoadScene("Main");
    }
}
