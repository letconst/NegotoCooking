using UnityEngine;

public class ToCutScene : MonoBehaviour
{
    private bool       flontCuttingBoardBool;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flontCuttingBoardBool = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            flontCuttingBoardBool = false;
        }
    }

    private void Update()
    {
        // インタラクトで調理移行
        if (!flontCuttingBoardBool           || // インタラクト範囲内にいる
            !Input.GetButtonDown("Interact") || // インタラクトボタン押下
            !Time.timeScale.Equals(1)) return;  // ポーズ中ではない

        GameManager.Instance.PlayerPos    = player.transform.position;
        GameManager.Instance.PlayerRotate = player.transform.localEulerAngles;
        SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.CutScenes);
    }
}
