using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSystem : MonoBehaviour
{
    // スタートボタンを押したら実行する
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("TutorialScenes");
    }
    private void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            SceneManager.LoadScene("TutorialScenes");
        }
    }
}
