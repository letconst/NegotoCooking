using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NosieManager : MonoBehaviour
{
    //騒音のメーター
    [SerializeField]
    private Image noiseMator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.alertBool)
        {
            noiseMator.GetComponent<Image>().fillAmount -= 0.0005f * 0.01f;
            GameManager.Instance.NoiseMator += 0.0005f * 0.01f;
        }
    }
}
