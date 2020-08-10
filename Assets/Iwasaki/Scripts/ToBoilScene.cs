using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBoilScene : MonoBehaviour
{
    private bool flontPotBool;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flontPotBool = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flontPotBool = false;
        }
    }

    void Update()
    {
        if (flontPotBool && Input.GetKeyDown("joystick button 2") || flontPotBool && Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.PlayerPos = player.transform.position;
            GameManager.Instance.PlayerRotate = player.transform.localEulerAngles;
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.BoilScenes);
        }
    }
}
