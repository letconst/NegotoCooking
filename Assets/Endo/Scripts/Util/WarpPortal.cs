using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    [SerializeField, Tooltip("ワープ先のポジション")]
    private Vector3 warpTo;

    [SerializeField, Tooltip("ワープ後の向き (Y)")]
    private float yRotationAfterWarp;

    [SerializeField, Tooltip("ワープ後の向きをワープ時の向きにするか否か")]
    private bool isLookSameRotation;

    [SerializeField, Tooltip("ワープ後にプレイヤーの移動を抑制する時間")]
    private float waitTimeAfterWarp;

    // 抑制時間のカウンター
    private float _waitTimeCounter;

    // ワープが実行されたか否か
    private bool _isWarped;

    private void Update()
    {
        // ワープ後にプレイヤーの行動を抑制
        if (_isWarped)
        {
            _waitTimeCounter += Time.deltaTime;

            Player.Instance.isStop = _isWarped = _waitTimeCounter < waitTimeAfterWarp;
        }
        // 抑制時間が過ぎたらカウンターをリセット
        else if (_waitTimeCounter > waitTimeAfterWarp)
        {
            _waitTimeCounter = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ワープ時の向きにするようになっていたらそちらを使用
            if (isLookSameRotation)
            {
                yRotationAfterWarp = other.transform.rotation.y;
            }

            other.gameObject.SetActive(false);

            other.transform.position = warpTo;
            other.transform.rotation = Quaternion.Euler(new Vector3(0, yRotationAfterWarp));

            other.gameObject.SetActive(true);
        }

        _isWarped = true;
    }
}
