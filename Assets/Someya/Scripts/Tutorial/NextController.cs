using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextController : MonoBehaviour
{
    public void OnClickNextButton()
    {
        SceneManager.LoadScene("GameScenes");
    }
    private void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            SceneManager.LoadScene("GameScenes");
        }
    }
}
