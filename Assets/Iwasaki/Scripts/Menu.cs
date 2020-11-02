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

    private CanvasGroup _playerInvCanvasGroup;
    private CanvasGroup _noiseGuageCanvasGroup;
    private CanvasGroup _dogToyCountCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        _playerInvCanvasGroup   = playeInv.GetComponent<CanvasGroup>();
        _noiseGuageCanvasGroup  = noiseGauge.GetComponent<CanvasGroup>();
        _dogToyCountCanvasGroup = dogToyCount.GetComponent<CanvasGroup>();

        menu.SetActive(false);

        if (GameManager.Instance.doOnce) return;
        GameManager.Instance.doOnce = true;
        GameManager.Instance.menuBool = true;

        if (GameManager.Instance.menuBool)
        {
            _playerInvCanvasGroup.alpha   = 0;
            _noiseGuageCanvasGroup.alpha  = 0;
            _dogToyCountCanvasGroup.alpha = 0;

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
            menu.SetActive(false);
            Time.timeScale = 1;
            
            _playerInvCanvasGroup.alpha = 1;
            _noiseGuageCanvasGroup.alpha = 1;
            _dogToyCountCanvasGroup.alpha = 1;
        }
    }
}
