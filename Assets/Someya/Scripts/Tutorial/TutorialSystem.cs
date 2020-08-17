using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSystem : MonoBehaviour
{
    public void OnClickStartButton()
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
