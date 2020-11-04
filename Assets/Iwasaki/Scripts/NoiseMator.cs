using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMator : MonoBehaviour
{
    private Image _mator;

    void Start()
    {
        _mator = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        _mator.fillAmount = 1 - GameManager.Instance.NoiseMator;

        if (_mator.fillAmount == 0)
        {
            // Debug.Log("Failed: 睡眠ゲージ0");
            GameManager.Instance.FailCount++;
            MasterController.Instance.Judgement();
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.Result);
        }
    }
}
