using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingScript : MonoBehaviour
{
    Slider _slider;
    void Start()
    {
        // スライダーを取得する
        _slider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    float _hp = 0;
    void Update()
    {
        // HP上昇
        _hp += 0.1f;
        // HPゲージに値を設定
        _slider.value = _hp;
    }
}
