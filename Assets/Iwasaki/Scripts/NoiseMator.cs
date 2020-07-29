using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMator : MonoBehaviour
{
    private float noiseValue;
    void Start()
    {
        noiseValue = this.gameObject.GetComponent<Image>().fillAmount;
        noiseValue = 1 - GameManager.Instance.NoiseMator;
    }

    private void Update()
    {
        if(noiseValue <= 0 || GameManager.Instance.NoiseMator >= 1)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
        }
    }
}
