using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireControl : MonoBehaviour
{
    [SerializeField]
    private Button _weakButton = null;
    [SerializeField]
    private Button _mediumButton = null;
    [SerializeField]
    private Button _strengthButton = null;
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

    public Text text;
    [HideInInspector]
    public Slider _slider;
    //騒音メーター
    [SerializeField]
    private Image noiseMator;

    private bool _isMove = false;
    private bool _isMoveup = true;
    private bool _isMovevereup = false;

    private int fireChange = 1;

    //焼き処理が終わったか
    [HideInInspector]
    static public bool bakeBool;
    //今焼き処理中か
    [HideInInspector]
    static public bool clickBool = true;
    void Start()
    {                
        // スライダーを取得する
        _slider = GameObject.Find("Slider").GetComponent<Slider>();             
    }

    void Update()
    {
        float dph = Input.GetAxis("D_Pad_H");

        if (dph <= 0)
        {
            if(fireChange <= 0)
            {
                fireChange = 0;
                return;
            }
            fireChange--;
        }
        if(dph >= 0)
        {
            if (fireChange >= 2)
            {
                fireChange = 2;
                return;
            }
            fireChange++;
        }

        if (_slider.value >= 100)
        {
            _slider.value = 0;
        }

        if (clickBool == true) return;  

        if(fireChange == 0)
        {
            text.text = "弱火";
            //スライダーに値を設定
            _slider.value += yowabi;
            noiseMator.GetComponent<Image>().fillAmount -= noiseYowabi;
            GameManager.Instance.NoiseMator += noiseYowabi;
        }
        if (fireChange == 1)
        {
            text.text = "中火";
            _slider.value += tyubi;
            noiseMator.GetComponent<Image>().fillAmount -= noiseTyubi;
            GameManager.Instance.NoiseMator += noiseTyubi;
        }
        if (fireChange == 2)
        {
            text.text = "強火";
            _slider.value += tuyobi;
            noiseMator.GetComponent<Image>().fillAmount -= noiseTuyobi;
            GameManager.Instance.NoiseMator += noiseTuyobi;
        }
    }

    public void OnWeakButton()
    {
        text.text = "弱火";
        _isMove = true;
        _isMoveup = false;
        _isMovevereup = false;
    }

    public void OnMediumButton()
    {
        text.text = "中火";
        _isMoveup = true;
        _isMove = false;
        _isMovevereup = false;
    }

    public void OnStrengthButton()
    {
        text.text = "強火";
        _isMovevereup = true;
        _isMove = false;
        _isMoveup = false;
    }
}
