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
}
