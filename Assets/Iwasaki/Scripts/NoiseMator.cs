using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Slider>().value = 100 - GameManager.Instance.NoiseMator;
    }

    private void Update()
    {
        if(this.gameObject.GetComponent<Slider>().value <= 0 || GameManager.Instance.NoiseMator >= 100)
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
        }
    }
}
