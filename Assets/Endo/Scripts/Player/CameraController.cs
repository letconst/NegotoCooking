using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //プレイヤーを変数に格納
    private GameObject _player;
    private float _playerPos;
    //回転させるスピード
    public float rotateSpeed = 3.0f;

    public Player MyP { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        // プレイヤーオブジェクト取得
        //_player = GameObject.FindGameObjectWithTag("Player");
        MyP = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        var playerPos = MyP.transform.position;

        if (playerPos.z >= -22)
        {
            // カメラの位置をプレイヤーに追従
            transform.position = new Vector3(playerPos.x, playerPos.y + 15, playerPos.z - 15);
        }
        else
        {
            // カメラの位置をプレイヤーに追従
            transform.position = new Vector3(playerPos.x, playerPos.y + 15, -37);
        }

        //回転させる角度
        float angle = Input.GetAxis("R_Stick_H") * rotateSpeed;
 
        //プレイヤー位置情報
        Vector3 _playerPos = MyP.transform.position;

        //カメラを回転させる
        transform.RotateAround(playerPos, Vector3.up, angle);
    }
}
