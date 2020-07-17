using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float
        walkSpeed   = 10.0f,   // 移動速度
        sprintSpeed = 15.0f,   // ダッシュ速度
        rotateSpeed = 700.0f, // 回転速度
        gravity     = 50.0f;   // 重力

    private CharacterController _controller;
    private Vector3             _moveDirection = Vector3.zero;
    private bool _isTouch;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        // A押下でアイテム使用
        if (Input.GetKeyDown("joystick button 0"))
        {
            Debug.Log("A is clicked!");
        }

        // X押下でインタラクション
        if (Input.GetKeyDown("joystick button 2"))
        {
            Interaction();
            Debug.Log("X is clicked!");
        }

        // Y押下でアイテムドロップ
        if (Input.GetKeyDown("joystick button 3"))
        {
            DropItem();
            Debug.Log("Y is clicked!");
        }
    }

    /// <summary>
    /// コントローラー入力によるプレイヤーの移動を制御
    /// </summary>
    private void Movement()
    {
        Quaternion q;
        float
            _h = Input.GetAxis("Horizontal"), // 水平
            _v = Input.GetAxis("Vertical");   // 垂直

        // 接地判定
        if (_controller.isGrounded)
        {
            // Bボタン押下時にダッシュ、そうでなければ歩く
            _moveDirection = Input.GetKey("joystick button 1")
                ? new Vector3(_h * sprintSpeed, 0, _v * sprintSpeed)
                : new Vector3(_h * walkSpeed, 0, _v * walkSpeed);
        }
        else
        {
            // 一定以上落下時に定位置へ戻す
            if (_moveDirection.y < -100)
            {
                this.transform.position = new Vector3(500, 0, 0);
            }
            else
            {
                // 重力による落下
                _moveDirection.y -= gravity * Time.deltaTime;
                //Debug.Log(moveDirection.y.ToString());
            }
        }

        // 移動
        _controller.Move(_moveDirection * Time.deltaTime);


        // 入力時に向きを変更
        if (_h != 0 || _v != 0)
        {
            q = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
        }
    }

    private void Interaction()
    {
        
    }

    private void DropItem()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.CompareTo("Kitchen") == 0)
        {
            if (Input.GetMouseButtonDown(0) && !_isTouch)
            {
                _isTouch = true;
                // シーンを追加
                SceneManager.LoadSceneAsync("CookingScenes", LoadSceneMode.Additive);
            }
            if(Input.GetMouseButtonDown(1) && !_isTouch)
            {
                _isTouch = false;
            }
        }

    }
    
}
