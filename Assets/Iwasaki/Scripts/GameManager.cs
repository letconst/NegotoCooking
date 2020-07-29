using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : SingletonMonoBehaviour<GameManager>
{    
    private float noiseValue;

    public float NoiseMator
    {
        set
        {
            noiseValue = Mathf.Clamp(value, 0, 100);
        }
        get
        {
            return noiseValue;
        }
    }

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
