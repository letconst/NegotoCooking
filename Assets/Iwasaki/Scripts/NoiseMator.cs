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
            MasterController.Instance.Judgement();
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.Result);
        }
    }
}
