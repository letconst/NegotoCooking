using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    // プレイヤーの各パラメーター
    [SerializeField, Tooltip("移動速度")]
    private float walkSpeed;

    [SerializeField, Tooltip("ダッシュ速度")]
    private float sprintSpeed;

    [SerializeField, Tooltip("重力")]
    private float gravity;

    [SerializeField, Tooltip("回転速度")]
    private int rotateSpeed;

    // プレイヤーが停止状態か否か
    public bool isStop;

    // ダッシュボタンが入力され、移動し続けているか否か
    private bool                _isRecieveSprint;
    private CharacterController _controller;
    private Vector3             _moveDirection = Vector3.zero;
    private Animator            _animator;
    private float               _currentMoveSpeed;
    private float               _maxWalkSpeed   = .6f;
    private float               _maxSprintSpeed = 1f;
    private float               _animDeltaTime  = .05f;

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
        // 水平入力
        var h = (isStop)
                    ? 0
                    : Input.GetAxis("Horizontal");

        // 垂直入力
        var v = (isStop)
                    ? 0
                    : Input.GetAxis("Vertical");

        // ダッシュ入力受付
        _isRecieveSprint = _isRecieveSprint ||
                           Input.GetButton("Sprint");

        // ダッシュ入力の有無で移動速度を変動
        var moveSpeed = (_isRecieveSprint)
                            ? sprintSpeed
                            : walkSpeed;

        // 接地判定
        if (_controller.isGrounded)
        {
            _moveDirection = new Vector3(h * moveSpeed, 0, v * moveSpeed);
        }
        else
        {
            // 一定以上落下時に定位置へ戻す
            if (!(_moveDirection.y < -100))
            {
                // 重力による落下
                _moveDirection.y -= gravity * Time.deltaTime;
            }
        }

        // 移動
        _controller.Move(_moveDirection * Time.deltaTime);

        // 移動入力時
        if (h != 0 ||
            v != 0)
        {
            // 向きを変更
            var rot = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);

            // アニメーション速度設定
            _currentMoveSpeed = (_isRecieveSprint)
                                    ? Mathf.Clamp(_currentMoveSpeed + _animDeltaTime, 0, _maxSprintSpeed)
                                    : Mathf.Clamp(_currentMoveSpeed + _animDeltaTime, 0, _maxWalkSpeed);
        }
        else
        {
            _currentMoveSpeed = Mathf.Clamp(_currentMoveSpeed - _animDeltaTime, 0, _maxSprintSpeed);
        }

        //アニメーション
        _animator.SetFloat(Animator.StringToHash("Speed"), _currentMoveSpeed);

        // ダッシュ入力があっても移動してなければダッシュ解除
        if (_isRecieveSprint && (h == 0 && v == 0))
        {
            _isRecieveSprint = false;
        }
    }
}
