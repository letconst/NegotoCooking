using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSystem : MonoBehaviour
{
    // スタートボタンを押したら実行する
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("GameScenes");
        // オープニングのシーンへ
        //SceneManager.LoadScene("OpeningScene");
    }
    private void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            SceneManager.LoadScene("GameScenes");
        }
    }
}
