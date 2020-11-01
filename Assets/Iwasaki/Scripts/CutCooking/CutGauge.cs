using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutGauge : MonoBehaviour
{
    private bool stopGaugeBool;
    [SerializeField]
    private Slider cutGauge;
    [SerializeField]
    private float sliderSpeed;
    private bool sliderBool = true;
    //切り処理が終わったか
    [HideInInspector]
    static public bool cutBool;
    //今切り処理中か
    [HideInInspector]
    static public bool clickBool = true;
    [HideInInspector]
    static public bool cantBackBool = true;
    //切り終わるまでのメーター
    public Slider _slider;
    ////騒音のメーター
    //[SerializeField]
    //private Image noiseMator;
    private bool doOnce = true;
    private bool toRightBool = true;
    void Start()
    {
        cutGauge.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // ポーズ中は進行および操作させない
        if (PushPause.Instance.IsNowPausing) return;

        if (Input.GetKeyDown("joystick button 1") && cantBackBool || Input.GetKeyDown(KeyCode.E) && cantBackBool)
        {
            SceneManager.LoadScene("GameScenes");
        }

        // 調理メーターがMAXになったら戻す
        if (_slider.value >= 100)
        {
            _slider.value = 0;
        }

        if (clickBool == true) return;

        if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.Q))
        {
            if (stopGaugeBool) return;
            stopGaugeBool = true;
            doOnce = true;
        }

        if (!stopGaugeBool)
        {
            //一番左と一番右どちらに棒があるか。
            if (cutGauge.value <= 0)
            {
                sliderBool = true;
                toRightBool = false;
            }
            if (cutGauge.value >= 100)
            {
                sliderBool = false;
            }

            if (sliderBool)
            {
                //一番右にもどす
                cutGauge.value = 100;
                StartCoroutine(waitTime(1.0f));
            }
            if (!sliderBool && toRightBool)
            {
                cutGauge.value -= sliderSpeed;
            }
        }
        if (stopGaugeBool && doOnce)
        {
            doOnce = false;
            SoundManager.Instance.PlaySe(SE.CutSound);
            if (cutGauge.value >= 48 && 52 >= cutGauge.value)
            {
                _slider.value += 20;
            }
            else if (cutGauge.value >= 41 && 47 >= cutGauge.value || cutGauge.value >= 53 && 59 >= cutGauge.value)
            {
                _slider.value += 15;
            }
            else if (cutGauge.value >= 33 && 40 >= cutGauge.value || cutGauge.value >= 60 && 67 >= cutGauge.value)
            {
                _slider.value += 10;
            }
            else if (cutGauge.value >= 20 && 32 >= cutGauge.value || cutGauge.value >= 68 && 80 >= cutGauge.value)
            {
                _slider.value += 5;
            }
            //一番右にもどす
            cutGauge.value = 100;
            StartCoroutine(waitTime(1.0f));
        }
    }
    IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        stopGaugeBool = false;
        toRightBool = true;
        yield break;
    }
}
