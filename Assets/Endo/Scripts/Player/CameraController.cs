using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _player;
    private float _playerPos;

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
    }
}
