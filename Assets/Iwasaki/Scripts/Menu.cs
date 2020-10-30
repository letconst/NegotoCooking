using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject noiseGauge;
    [SerializeField]
    private GameObject dogToyCount;
    [SerializeField]
    private GameObject playeInv;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            noiseGauge.SetActive(true);
            dogToyCount.SetActive(true);
            playeInv.SetActive(true);
        }
    }
}
