﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSystem : MonoBehaviour
{
    // スタートボタンを押したら実行する
    public void OnClickStartButton()
    {
        SoundManager.Instance.PlaySe(SE.Submit);
        SceneManager.LoadScene("TutorialScenes");
    }
}
