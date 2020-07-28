using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.Cooking_test);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Q))
        {
            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.CookingScenes);
        }
    }
}
