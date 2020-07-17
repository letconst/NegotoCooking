using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Endo: 以下、Unity側の変更点です
// 各ドアオブジェクトの親に、子オブジェクトのColliderを検出させるためRigidbodyを持たせました
// Rigidbodyの値の変更は、Use Gravityをオフに、Is Kinematicをオンにしたのみです
// 各子オブジェクトには、当たり判定に加え近くにいるかを検出させるため、Is TriggerなBox Colliderを追加で持たせました
// 検出範囲はXを600としていますが、変更させたい場合はこのBox ColliderのSize: Xをイジってください
// 観音開きのドアは、向かって左側のCenter: Zを-0.5、右側のCenter: Zを0.5にし、両側のSize: Zを2にしてください

public class DoorControl : MonoBehaviour
{
    [SerializeField]
    private Animator _door;
    // ドア付近にいるか否か
    private bool _isNear = false;

    // Update is called once per frame
    void Update()
    {
        // ドア付近にいる場合にXボタン押下で開閉
        if (Input.GetKeyDown("joystick button 2") && _isNear)
        {
            // ドアのステータス変化が1と2の2パターンしかないのであれば、値をboolで持つようにしたほうが良いかと思います
            // もしそのように変更が可能なら、以下の構文を使用してください
            // _door.SetBool("DoorStatus", !_door.GetBool("DoorStatus"));
            _door.SetInteger("DoorStatus", _door.GetInteger("DoorStatus") == 1 ? 2 : 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") _isNear = false;
    }
}
