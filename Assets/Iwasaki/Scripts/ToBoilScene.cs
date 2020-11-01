using UnityEngine;

public class ToBoilScene : MonoBehaviour
{
    private bool       flontPotBool;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flontPotBool = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flontPotBool = false;
        }
    }

    private void Update()
    {
        // インタラクトで調理移行
        if (!flontPotBool                    ||      // インタラクト範囲内にいる
            !Input.GetButtonDown("Interact") ||      // インタラクトボタン押下
            PushPause.Instance.IsNowPausing) return; // ポーズ中ではない

        GameManager.Instance.PlayerPos    = player.transform.position;
        GameManager.Instance.PlayerRotate = player.transform.localEulerAngles;
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.BoilScenes);
    }
}
