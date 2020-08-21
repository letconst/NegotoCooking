using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    [SerializeField, Tooltip("ワープ先のポジション")]
    private Vector3 _warpTo;

    [SerializeField, Tooltip("ワープ後の向き (Y)")]
    private float _yRotationAfterWarp;

    [SerializeField, Tooltip("ワープ後の向きをワープ時の向きにするか否か")]
    private bool _isLookSameRotation;

    [SerializeField, Tooltip("ワープ後にプレイヤーの移動を抑制する時間")]
    private float _waitTimeAfterWarp;

    // 抑制時間のカウンター
    private float _waitTimeCounter = 0;

    // ワープが実行されたか否か
    private bool _isWarped = false;

    private void Update()
    {
        // ワープ後にプレイヤーの行動を抑制
        if (_isWarped)
        {
            _waitTimeCounter += Time.deltaTime;

            Player.Instance.isStop = _isWarped = _waitTimeCounter < _waitTimeAfterWarp;
        }
        // 抑制時間が過ぎたらカウンターをリセット
        else if (_waitTimeCounter > _waitTimeAfterWarp)
        {
            _waitTimeCounter = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ワープ時の向きにするようになっていたらそちらを使用
            if (_isLookSameRotation)
            {
                _yRotationAfterWarp = other.transform.rotation.y;
            }

            other.gameObject.SetActive(false);

            other.transform.position = _warpTo;
            other.transform.rotation = Quaternion.Euler(new Vector3(0, _yRotationAfterWarp));

            other.gameObject.SetActive(true);
        }

        _isWarped = true;
    }
}
