using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : SingletonMonoBehaviour<Player>
{
    // プレイヤーの各パラメーター
    [SerializeField, Tooltip("移動速度")]
    private float _walkSpeed;

    [SerializeField, Tooltip("重力")]
    private float _gravity;

    [SerializeField, Tooltip("回転速度")]
    private int _rotateSpeed;

    // プレイヤーが停止状態か否か
    public  bool                IsStop = false;
    private CharacterController _controller;
    private Vector3             _moveDirection = Vector3.zero;
    private Animator            _animator;

    // Start is called before the first frame update
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator   = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
    }

    /// <summary>
    /// コントローラー入力によるプレイヤーの移動を制御
    /// </summary>
    private void Movement()
    {
        Quaternion rot;
        // 水平入力
        var h = (IsStop)
            ? 0
            : Input.GetAxis("Horizontal");
        // 垂直入力
        var v = (IsStop)
            ? 0
            : Input.GetAxis("Vertical"); ;

        // 接地判定
        if (_controller.isGrounded)
        {
            _moveDirection = new Vector3(h * _walkSpeed, 0, v * _walkSpeed);
        }
        else
        {
            // 一定以上落下時に定位置へ戻す
            if (!(_moveDirection.y < -100))
            {
                // 重力による落下
                _moveDirection.y -= _gravity * Time.deltaTime;
            }
        }

        // 移動
        _controller.Move(_moveDirection * Time.deltaTime);


        // 入力時に向きを変更
        if (h != 0 || v != 0)
        {
            rot = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotateSpeed);
        }

        //アニメーション
        if (h == 0 && v == 0)
        {
            _animator.SetBool("Walk", false);
        }
        else
        {
            _animator.SetBool("Walk", true);
        }
    }
}
