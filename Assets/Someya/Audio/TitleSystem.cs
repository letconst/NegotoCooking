using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{
    // タイトルボタンを押したら実行する
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("TitleScenes");
    }
}
