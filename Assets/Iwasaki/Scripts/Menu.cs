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
        menu.SetActive(false);

        if (GameManager.Instance.doOnce) return;
        GameManager.Instance.doOnce = true;
        GameManager.Instance.menuBool = true;

        if (GameManager.Instance.menuBool)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 0") && GameManager.Instance.menuBool || 
            Input.GetKeyDown(KeyCode.Q) && GameManager.Instance.menuBool)
        {
            GameManager.Instance.menuBool = false;
            Time.timeScale = 1;
            menu.SetActive(false);
            noiseGauge.GetComponent<CanvasGroup>().alpha = 1;
            dogToyCount.GetComponent<CanvasGroup>().alpha = 1;
            playeInv.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
