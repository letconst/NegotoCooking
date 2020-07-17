using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigurationSystem : MonoBehaviour
{
    // 設定ボタンを押したら実行する
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("AudioScenes");
    }
}
