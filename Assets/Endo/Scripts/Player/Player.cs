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

    [SerializeField, Tooltip("カメラのトランスフォーム")]
    private Transform camPos;

    [SerializeField, Tooltip("歩行アニメーションの最大スピード"), Range(0, 1)]
    private float maxWalkAnimSpeed;

    [SerializeField, Tooltip("ダッシュアニメーションの最大スピード"), Range(0, 1)]
    private float maxSprintAnimSpeed;

    [SerializeField, Tooltip("1フレームごとのアニメーションの変化量"), Range(0, 1)]
    private float animDeltaTime;

    // プレイヤーが停止状態か否か
    public bool isStop;

    private bool                _isReceiveSprint;              // ダッシュボタンが入力され、移動し続けているか否か
    private float               _currentMoveAnimSpeed;         // 現在のアニメーション速度
    private CharacterController _controller;                   // プレイヤー操作用
    private Vector3             _moveDirection = Vector3.zero; // 向いている方向
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
        // カメラの向いている方向の単位ベクトル
        var camForward = Vector3.Scale(camPos.forward, new Vector3(1, 0, 1)).normalized;

        // 水平入力
        var h = (isStop)
                    ? 0
                    : Input.GetAxis("Horizontal");

        // 垂直入力
        var v = (isStop)
                    ? 0
                    : Input.GetAxis("Vertical");

        // ダッシュ入力受付
        _isReceiveSprint = _isReceiveSprint ||
                           Input.GetButton("Sprint");

        // ダッシュ入力の有無で移動速度を変動
        var moveSpeed = (_isReceiveSprint)
                            ? sprintSpeed
                            : walkSpeed;

        // プレイヤーの移動ベクトル
        var moveVec = (camForward * (v * moveSpeed) + camPos.right * (h * moveSpeed)) / 10;

        // 接地判定
        if (_controller.isGrounded)
        {
            _moveDirection = new Vector3(moveVec.x * moveSpeed, 0, moveVec.z * moveSpeed);
        }
        else
        {
            // 重力による落下
            _moveDirection.y -= gravity * Time.deltaTime;
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
            _currentMoveAnimSpeed = Mathf.Clamp(_currentMoveAnimSpeed + animDeltaTime,
                                                0,
                                                (_isReceiveSprint) ? maxSprintAnimSpeed : maxWalkAnimSpeed);
        }
        else
        {
            _currentMoveAnimSpeed = Mathf.Clamp(_currentMoveAnimSpeed - animDeltaTime, 0, maxSprintAnimSpeed);
        }

        // アニメーション
        _animator.SetFloat(Animator.StringToHash("Speed"), _currentMoveAnimSpeed);

        // ダッシュ入力があっても移動してなければダッシュ解除
        if (_isReceiveSprint && (h == 0 && v == 0))
        {
            _isReceiveSprint = false;
        }
    }
}
