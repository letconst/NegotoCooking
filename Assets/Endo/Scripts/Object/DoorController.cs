using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class DoorController : MonoBehaviour
{
    [SerializeField, Tooltip("ドアの軸に付属するアニメーター")]
    private List<Animator> pivotAnims;

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

    // 近くにいるプレイヤーの座標
    private Vector3 _nearPlayerPos;

    // ドアを開くごとに設定するID
    private float _animProcessID;

    private static readonly int IsOpen  = Animator.StringToHash("IsOpen");
    private static readonly int IsFront = Animator.StringToHash("IsFront");

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
        // インタラクトでドア開閉
        if (!_isNear                         || // インタラクト範囲内にいる
            !Input.GetButtonDown("Interact") || // インタラクトボタン押下
            !Time.timeScale.Equals(1)) return;  // ポーズ中ではない

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

        foreach (var anim in pivotAnims)
        {
            anim.SetBool(IsOpen, !anim.GetBool(IsOpen));
        }

        // 指定時間経過後に自動で閉じる
        yield return new WaitForSeconds(closeLimit);

        // ドアが手動で閉じられなかったら自動で閉じる
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
        if (!other.CompareTag("Player")) return;

        _isNear        = true;
        _nearPlayerPos = other.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // キャッシュ
        var selfTrf = transform;

        // ドアからプレイヤーへの正規化ベクトル
        var normPlayer2Door = (_nearPlayerPos - selfTrf.position).normalized;

        // プレイヤーがドアの正面にいるか否か
        // プレイヤーは、ドアとの内積が正なら正面、負なら背面にいる
        var isPlayerInFrontOfDoor = Vector3.Dot(selfTrf.right, normPlayer2Door) > 0;

        // ドアの開く方向を指定
        foreach (var anim in pivotAnims)
        {
            anim.SetBool(IsFront, isPlayerInFrontOfDoor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isNear        = false;
        _nearPlayerPos = other.transform.position;
    }
}
