﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FireControl_boil : MonoBehaviour
{
    //火加減ごとの値
    [SerializeField]
    private float yowabi;
    [SerializeField]
    private float tyubi;
    [SerializeField]
    private float tuyobi;
    //ノイズメーターの値
    [SerializeField]
    private float noiseYowabi;
    [SerializeField]
    private float noiseTyubi;
    [SerializeField]
    private float noiseTuyobi;
    //火の強さを出すテキスト
    [SerializeField]
    private Text text;
    //焼き終わるまでのメーター
    public Slider _slider;
    //騒音のメーター
    [SerializeField]
    private Image noiseMator;

    [SerializeField]
    private GameObject FireChar;
    [SerializeField]
    private GameObject leftAllow;
    [SerializeField]
    private GameObject RightAllow;

    private bool doOnce = true;
    private int fireChange = 1;

    //煮込み処理が終わったか
    [HideInInspector]
    static public bool boilBool;
    //今焼き処理中か
    [HideInInspector]
    static public bool clickBool = true;
    [SerializeField]
    private GameObject Otama;
    [SerializeField]
    private GameObject Centerpostion;
    void Start()
    {
        // スライダーを取得する
        //_slider = GameObject.Find("Slider").GetComponent<Slider>();
        //中火の色にしておく
        FireChar.GetComponent<Image>().color = Color.yellow;
    }

    void Update()
    {
        if (Input.GetKeyDown("joystick button 1") && _slider.value == 0)
        {
            SceneManager.LoadScene("GameScenes");
        }

        float dph = Input.GetAxis("D_Pad_H");
        float Stick_V = Input.GetAxis("Vertical");

        if (dph < 0 && fireChange != 0)
        {
            if (fireChange <= 0)
            {
                fireChange = 0;
                return;
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(WaitForSeconds(1.0f));
                fireChange--;
            }
        }
        if (dph > 0 && fireChange != 2)
        {
            if (fireChange >= 2)
            {
                fireChange = 2;
                return;
            }

            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(WaitForSeconds(1.0f));
                fireChange++;
            }
        }

        if (_slider.value >= 100)
        {
            _slider.value = 0;
        }

        if (clickBool == true) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Debug.Log(h + "," + v);
        Otama.transform.position = new Vector3(Centerpostion.transform.position.x + h * 65, Centerpostion.transform.position.y, Centerpostion.transform.position.z + v * 70);

        if (fireChange <= 0)
        {
            leftAllow.gameObject.SetActive(false);
            FireChar.GetComponent<Image>().color = Color.cyan;
            text.text = "弱";
            //スライダーに値を設定
            _slider.value += yowabi;
            noiseMator.GetComponent<Image>().fillAmount -= noiseYowabi * 0.01f;
            GameManager.Instance.NoiseMator += noiseYowabi * 0.01f;
        }
        else if (fireChange == 1)
        {
            leftAllow.gameObject.SetActive(true);
            RightAllow.gameObject.SetActive(true);
            FireChar.GetComponent<Image>().color = Color.yellow;
            text.text = "中";
            _slider.value += tyubi;
            noiseMator.GetComponent<Image>().fillAmount -= noiseTyubi * 0.01f;
            GameManager.Instance.NoiseMator += noiseTyubi * 0.01f;
        }
        else if (fireChange >= 2)
        {
            RightAllow.gameObject.SetActive(false);
            FireChar.GetComponent<Image>().color = Color.red;
            text.text = "強";
            _slider.value += tuyobi;
            noiseMator.GetComponent<Image>().fillAmount -= noiseTuyobi * 0.01f;
            GameManager.Instance.NoiseMator += noiseTuyobi * 0.01f;
        }
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        doOnce = true;
        yield break;
    }
}