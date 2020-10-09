using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    //カウントアップ
    public static float CountUp = 500.0f;

    public static float CurrentTime = CountUp;

    //タイムリミット
    [SerializeField]
    private float timeLimit;

    //時間を表示するText型の変数
    [SerializeField]
    private Text timeText;

    // Update is called once per frame
    private void Update()
    {
        //時間をカウントする
        CurrentTime -= Time.deltaTime;

        //時間を表示する
        timeText.text = CurrentTime.ToString("0");

        // タイムアップでゲームオーバー
        if (CurrentTime <= timeLimit)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
        }
    }
}
