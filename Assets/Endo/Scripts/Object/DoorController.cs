using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private float closeLimit;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    private Animator _anim;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    // Start is called before the first frame update
    private void Start()
    {
        _anim = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ドア開閉
        if (_isNear && (Input.GetKeyDown("joystick button 2") ||
                        Input.GetKeyDown(KeyCode.E)))
        {
            SwitchOpen();

            // 指定時間経過後に自動で閉じる
            Invoke(nameof(SwitchOpen), closeLimit);

            // 閉じたときは自動で開かないようにする
            if (!_anim.GetBool(IsOpen))
            {
                CancelInvoke(nameof(SwitchOpen));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = false;
    }

    /// <summary>
    /// ドアを開閉する
    /// </summary>
    private void SwitchOpen()
    {
        _anim.SetBool(IsOpen, !_anim.GetBool(IsOpen));
    }
}
