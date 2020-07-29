using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMator : MonoBehaviour
{
    void Start()
    {

        this.gameObject.GetComponent<Image>().fillAmount = 1 - GameManager.Instance.NoiseMator;
    }

    private void Update()
    {
        if(this.gameObject.GetComponent<Image>().fillAmount <= 0 || GameManager.Instance.NoiseMator >= 1)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
        }
    }
}
