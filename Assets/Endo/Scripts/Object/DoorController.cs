using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField, Tooltip("インタラクト範囲のコライダー")]
    private Collider interactRange;

    [SerializeField, Tooltip("ドアの軸に付属するアニメーター")]
    private Animator pivotAnim;

    [SerializeField, Tooltip("ドアが自動で閉じるまでの時間（秒）")]
    private float closeLimit;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    // Update is called once per frame
    private void Update()
    {
        // ドア開閉
        if (_isNear &&
            Input.GetButtonDown("Interact"))
        {
            SwitchOpen();

            // 指定時間経過後に自動で閉じる
            Invoke(nameof(SwitchOpen), closeLimit);

            // 閉じたときは自動で開かないようにする
            if (!pivotAnim.GetBool(IsOpen))
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
        pivotAnim.SetBool(IsOpen, !pivotAnim.GetBool(IsOpen));
    }
}
