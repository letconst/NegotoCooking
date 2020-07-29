using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenControl : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown("joystick button 1"))
        {
            SceneManager.LoadScene("GameScenes");
        }
    }       
}
