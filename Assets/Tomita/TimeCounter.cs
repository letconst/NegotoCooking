using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    //カウントアップ
    public static float countup = 300.0f;

    //タイムリミット
    public float timeLimit = 0.0f;

    //時間を表示するText型の変数
    public Text timeText;

    // Update is called once per frame
    void Update()
    {
        //時間をカウントする
        countup -= Time.deltaTime;

        //時間を表示する
        timeText.text = countup.ToString("0");

        // タイムアップでゲームオーバー
        if (countup <= timeLimit)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
        }
    }
}