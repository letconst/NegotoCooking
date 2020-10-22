using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// TODO: プレイヤーがいる方向に対する側へ開くようにする
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class DoorController : MonoBehaviour
{
    [SerializeField, Tooltip("ドアの軸に付属するアニメーター")]
    private List<Animator> pivotAnims;

    // [SerializeField, Tooltip("ドアが開閉する最大角度")]
    // private float maxAnimAngle;

    [SerializeField, Tooltip("ドアが自動で閉じるまでの時間（秒）")]
    private float closeLimit;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField, Tooltip("ドアを開いた際のSE")]
    private AudioClip openSound;

    [SerializeField, Tooltip("ドアを閉じた際のSE")]
    private AudioClip closeSound;

    // プレイヤーが近くにいるか否か
    private bool _isNear;

    // ドアを開くごとに設定するID
    private float _animProcessID;

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
    }

    /// <summary>
    /// ドアを開閉する
    /// </summary>
    private IEnumerator SwitchOpen()
    {
        // IDを生成
        var rnd = Random.value;
        _animProcessID = rnd;

        // ドアがアイドル状態になったらアニメーションを実行
        yield return new WaitUntil(() =>
        {
            return pivotAnims.All(
                a => a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "DoorIdle");
        });

        // SEが鳴ってる間は待機
        yield return new WaitWhile(() => audioSource.isPlaying);

        // _isOpen = !_isOpen;

        // 開く際はアニメーション前の回転を記憶
        // if (_isOpen) _beforeRot = targetDoor.transform.rotation;

        // _afterRot = (_isOpen)
        //                 ? Quaternion.Inverse(Quaternion.Euler(0, maxAnimAngle, 0))
        //                 : _beforeRot * Quaternion.Euler(0, -maxAnimAngle, 0);

        foreach (var anim in pivotAnims)
        {
            anim.SetBool(IsOpen, !anim.GetBool(IsOpen));
        }

        // 指定時間経過後に自動で閉じる
        yield return new WaitForSeconds(closeLimit);

        // ドアが開いている状態であり、IDが同じなら（手動で閉じた後に再度開いていなければ）閉じる
        foreach (var anim in pivotAnims.Where(a => a.GetBool(IsOpen) && _animProcessID.Equals(rnd)))
        {
            anim.SetBool(IsOpen, false);
        }
    }

    /// <summary>
    /// 開くSEを再生する
    /// </summary>
    public void PlayOpenSound()
    {
        // 重複再生防止
        // 連打されると鳴らなくなることがあるので一時CO
        // if (audioSource.isPlaying) return;

        audioSource.PlayOneShot(openSound);
    }

    /// <summary>
    /// 閉じるSEを再生する
    /// </summary>
    public void PlayCloseSound()
    {
        // 重複再生防止
        if (audioSource.isPlaying) return;

        audioSource.PlayOneShot(closeSound);
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
