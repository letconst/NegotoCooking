using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _player;
    private Player p;

    public Player MyP { get => p; private set => p = value; }

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーオブジェクト取得
        //_player = GameObject.FindGameObjectWithTag("Player");
        MyP = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // カメラの位置をプレイヤーに追従
        transform.position = new Vector3(MyP.transform.position.x, MyP.transform.position.y + 15, MyP.transform.position.z - 3);
    }
}
