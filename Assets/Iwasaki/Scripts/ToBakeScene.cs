using UnityEngine;

public class ToBakeScene : MonoBehaviour
{
    private bool       flontHobBool;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flontHobBool = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flontHobBool = false;
        }
    }

    private void Update()
    {
        // インタラクトで調理移行
        if (!flontHobBool                    ||      // インタラクト範囲内にいる
            !Input.GetButtonDown("Interact") ||      // インタラクトボタン押下
            PushPause.Instance.IsNowPausing) return; // ポーズ中ではない

        GameManager.Instance.PlayerPos    = player.transform.position;
        GameManager.Instance.PlayerRotate = player.transform.localEulerAngles;
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.BakeScenes);
    }
}
