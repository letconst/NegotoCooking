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
    private Slider nosieMator;
    private bool _isMove = false;
    private bool _isMoveup = false;
    private bool _isMovevereup = false;

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

    float _hp = 0;

    void Update()
    {
        if(_slider.value >= 100)
        {           
            _slider.value = 0;
            _isMove = false;
            _isMoveup = false;
            _isMovevereup = false;
        }
        if(_isMove)
        {
            //スライダーに値を設定
            _slider.value += yowabi;
            nosieMator.value -= noiseYowabi;
            GameManager.Instance.NoiseMator += noiseYowabi;
        }
        if (_isMoveup)
        {            
            _slider.value += tyubi;
            nosieMator.value -= noiseTyubi;
            GameManager.Instance.NoiseMator += noiseTyubi;
        }
        if (_isMovevereup)
        {            
            _slider.value += tuyobi;
            nosieMator.value -= noiseTuyobi;
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
