using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootstepController : MonoBehaviour
{
    [SerializeField]
    private List<FootstepPair> footstepPairs = new List<FootstepPair>();

    [SerializeField, Tooltip("↑に未設定の床を歩いた際の足音")]
    private SE defaultFootstep;

    [SerializeField, Tooltip("足音の最低音量"), Range(0, 1)]
    private float minVolume;

    [SerializeField, Tooltip("足音の最大音量"), Range(0, 1)]
    private float maxVolume;

    [SerializeField, Tooltip("足音の最低ピッチ"), Range(0, 3)]
    private float minPitch;

    [SerializeField, Tooltip("足音の最大ピッチ"), Range(0, 3)]
    private float maxPitch;

    /// <summary>
    /// 足元の床に対応した足音を再生する
    /// </summary>
    public void PlaySound()
    {
        var volume = Random.Range(minVolume, maxVolume);
        var pitch  = Random.Range(minPitch, maxPitch);

        // 足元のオブジェクトをチェック
        Physics.Raycast(transform.position, Vector3.down, out var hit);

        // タグに応じてSE設定
        var se = footstepPairs.FirstOrDefault(pair => hit.collider.CompareTag(pair.FloorTagName))
                              ?.Se ??
                 // タグ未指定またはSE未設定のタグの場合は既定SEを鳴らす
                 defaultFootstep;

        SoundManager.Instance.PlaySe(se, volume, pitch);
    }
}

[System.Serializable]
public class FootstepPair
{
    [SerializeField]
    private string floorTagName;

    [SerializeField]
    private SE se;

    public string FloorTagName => floorTagName;

    public SE Se => se;
}
