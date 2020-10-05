using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class gotitle : MonoBehaviour
{
    [SerializeField]
    private GameObject titleButton;
   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(titleButton.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.Q))
            {
                SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.TitleScenes);

            }
        }
        
    }
    // Start is called before the first frame update


}
