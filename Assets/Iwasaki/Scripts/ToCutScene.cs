using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToCutScene : MonoBehaviour
{
    private bool flontCuttingBoardBool;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flontCuttingBoardBool = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flontCuttingBoardBool = false;
        }
    }

    void Update()
    {
        if (flontCuttingBoardBool && Input.GetKeyDown("joystick button 2") || flontCuttingBoardBool && Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.PlayerPos = player.transform.position;
            GameManager.Instance.PlayerRotate = player.transform.localEulerAngles;
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.CutScenes);
        }
    }
}
