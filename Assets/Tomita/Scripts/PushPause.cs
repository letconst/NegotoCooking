using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPause : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject pauseCanvas;
    private void Start()
    {
        pauseCanvas = GameObject.FindGameObjectWithTag("pauseCanvas");
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.M))
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
