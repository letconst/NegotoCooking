using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    //カウントアップ
    public static float Countup = 400.0f;

    [System.NonSerialized]
    public static float CurrentTime = Countup;

    //タイムリミット
    public float TimeLimit = 0.0f;

    //時間を表示するText型の変数
    public Text TimeText;

    // Update is called once per frame
    private void Update()
    {
        //時間をカウントする
        CurrentTime -= Time.deltaTime;

        //時間を表示する
        TimeText.text = CurrentTime.ToString("0");

        // タイムアップでゲームオーバー
        if (CurrentTime <= TimeLimit)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
        }
    }
}