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

        if (MyP.transform.position.z >= -22)
        {
            // カメラの位置をプレイヤーに追従
            transform.position = new Vector3(MyP.transform.position.x, MyP.transform.position.y + 15, MyP.transform.position.z - 15);
        }
        else
        {
            // カメラの位置をプレイヤーに追従
            transform.position = new Vector3(MyP.transform.position.x, MyP.transform.position.y + 15, -37);
        }
    }
}
