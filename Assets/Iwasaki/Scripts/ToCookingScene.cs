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
        if (flontHobBool && Input.GetKeyDown(KeyCode.Q) || flontHobBool && Input.GetKeyDown("joystick button 2"))
        {
            PlayerPositioning.lastPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);            
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.CookingScenes);
        }
    }
}
