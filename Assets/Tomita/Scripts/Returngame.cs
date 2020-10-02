using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Returngame : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown("joystick button 2") ||   Input.GetKeyDown(KeyCode.Q))
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameScenes);
        }
    }
}
