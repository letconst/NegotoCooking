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

    public Text text;
    public Slider _slider;
    private bool _isMove = false;
    private bool _isMoveup = false;
    private bool _isMovevereup = false;
    void Start()
    {
        // スライダーを取得する
        _slider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    float _hp = 0;

    void Update()
    {
       if(_isMove)
        {
            _hp += 0.03f;
            //HPゲージに値を設定
            _slider.value = _hp;
        }
        if (_isMoveup)
        {
            _hp += 0.04f;
            //HPゲージに値を設定
            _slider.value = _hp;
        }
        if (_isMovevereup)
        {
            _hp += 0.05f;
            //HPゲージに値を設定
            _slider.value = _hp;
        }
    }

    public void OnWeakButton()
    {
        text.text = "弱火";
        _isMove = true;
    }

    public void OnMediumButton()
    {
        text.text = "中火";
        _isMoveup = true;
    }

    public void OnStrengthButton()
    {
        text.text = "強火";
        _isMovevereup = true;
    }
}
