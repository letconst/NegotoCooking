using System.Collections;
using UnityEngine;

// TODO: プレイヤーがいる方向に対する側へ開くようにする
[RequireComponent(typeof(Collider))]
public class DoorController : MonoBehaviour
{
    [SerializeField, Tooltip("ドアの軸に付属するアニメーター")]
    private Animator pivotAnim;

    // [SerializeField, Tooltip("ドアが開閉する最大角度")]
    // private float maxAnimAngle;

    [SerializeField, Tooltip("ドアが自動で閉じるまでの時間（秒）")]
    private float closeLimit;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    // ドアが開いているか否か
    // private bool _isOpen;

    // ドアが開く前の回転
    // private Quaternion _beforeRot;

    // ドアが開いた後の回転
    // private Quaternion _afterRot;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    // Update is called once per frame
    private void Update()
    {
        InputHandler();
    }

    /// <summary>
    /// 入力操作を処理する
    /// </summary>
    private void InputHandler()
    {
        // プレイヤーが範囲内におり、インタラクト入力があったときのみ動作
        if (!_isNear ||
            !Input.GetButtonDown("Interact")) return;

        StartCoroutine(nameof(SwitchOpen));

        // 指定時間経過後に自動で閉じる
        Invoke(nameof(SwitchOpen), closeLimit);

        // 閉じたときは自動で開かないようにする
        if (!pivotAnim.GetBool(IsOpen))
        {
            CancelInvoke(nameof(SwitchOpen));
        }
    }

    /// <summary>
    /// ドアを開閉する
    /// </summary>
    private IEnumerator SwitchOpen()
    {
        var curClipName = pivotAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        // ドアがアイドル状態になったらアニメーションを実行
        yield return new WaitUntil(() => curClipName == "DoorIdle");

        // _isOpen = !_isOpen;

        // 開く際はアニメーション前の回転を記憶
        // if (_isOpen) _beforeRot = targetDoor.transform.rotation;

        // _afterRot = (_isOpen)
        //                 ? Quaternion.Inverse(Quaternion.Euler(0, maxAnimAngle, 0))
        //                 : _beforeRot * Quaternion.Euler(0, -maxAnimAngle, 0);

        pivotAnim.SetBool(IsOpen, !pivotAnim.GetBool(IsOpen));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = false;
    }
}
