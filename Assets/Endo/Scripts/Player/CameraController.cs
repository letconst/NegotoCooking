using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("回転スピード")]
    private float rotateSpeed = 3.0f;

    [SerializeField, Tooltip("Zの移動を固定する座標値")]
    private float minFrontPos;

    // プレイヤーオブジェクト
    private GameObject _player;

    // カメラの親（軸）オブジェクト
    private GameObject _wrapper;

    // カメラの角度

    // Start is called before the first frame update
    private void Start()
    {
        _player  = GameObject.FindGameObjectWithTag("Player");
        _wrapper = transform.parent.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        var playerPos = _player.transform.position;

        if (playerPos.z >= minFrontPos)
        {
            // 軸をプレイヤーに追従
            _wrapper.transform.position = playerPos;
        }
        else
        {
            // 軸をプレイヤーに追従
            _wrapper.transform.position = new Vector3(playerPos.x, playerPos.y, minFrontPos);
        }

        //回転させる角度
        float angle = Input.GetAxis("R_Stick_H") * rotateSpeed;

        //カメラを回転させる
        transform.RotateAround(playerPos, Vector3.up, angle);

    }
}
