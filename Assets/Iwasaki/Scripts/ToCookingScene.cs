using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCookingScene : MonoBehaviour
{
    private bool flontHobBool;
    [SerializeField]
    private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flontHobBool = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flontHobBool = false;
        }
    }

    void Update()
    {
        if (flontHobBool && Input.GetKeyDown("joystick button 2"))
        {
            GameManager.Instance.PlayerPos = player.transform.position;
            GameManager.Instance.PlayerRotate = player.transform.localEulerAngles;
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.CookingScenes);
        }
    }
}
