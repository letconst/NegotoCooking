using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NegotoController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTrf;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField, Tooltip("寝言のフェード時間"), Range(0, 1)]
    private float deltaFade;

    [SerializeField, Tooltip("寝言のフェードインの最低遅延"), Range(0, 1)]
    private float deltaMinFadeInDelay;

    [SerializeField, Tooltip("寝言のフェードインの最大遅延"), Range(0, 1)]
    private float deltaMaxFadeInDelay;

    [SerializeField, Tooltip("寝言の最低透明度"), Range(0, 1)]
    private float minTransparency;

    [SerializeField, Tooltip("寝言の最大透明度"), Range(0, 1)]
    private float maxTransparency;

    // 達成に必要な食材
    public Item requireFood;

    // 達成に必要な食材の状態
    public List<FoodState> requireStates;

    // レシピリストのコピー
    private List<RequireFoods> _recipe;

    // プレイヤーが表示範囲に入ったか否か
    private bool _isEntered;

    // 寝言表示のフェードインの遅延量
    private float _fadeInDelay;

    // プレイヤーが寝言表示範囲に入ってからの経過時間
    private float _elapsedTimeForDelay;

    // Start is called before the first frame update
    private void Start()
    {
        _recipe   = NegotoManager.Instance.Recipe;

        // 初期表示のアイテムをランダムに選定
        var foodIndex = Random.Range(0, _recipe.Count);
        requireFood   = _recipe[foodIndex].Food;
        requireStates = _recipe[foodIndex].States;
        _recipe.RemoveAt(foodIndex);

        textMesh.text = requireFood.ItemName;
    }

    // Update is called once per frame
    private void Update()
    {
        // 常にカメラに向ける
        transform.forward = cameraTrf.forward;

        // 表示のフェードイン・アウト
        FadeDisplay();
    }

    /// <summary>
    /// 寝言の表示をフェードイン・アウトする
    /// </summary>
    private void FadeDisplay()
    {
        var tmpSpriteColor = spriteRenderer.color;
        var tmpTextColor   = textMesh.color;

        // 寝言表示範囲にプレイヤーがいれば表示
        if (NegotoManager.Instance.IsPlayerNeared)
        {
            // 範囲内に入るごとの初回時に、フェードインまでの遅延時間を設定
            if (!_isEntered)
            {
                _isEntered   = true;
                _fadeInDelay = Random.Range(deltaMinFadeInDelay, deltaMaxFadeInDelay);
            }

            // 遅延時間が経過するまで寝言を表示しない
            if (_elapsedTimeForDelay < _fadeInDelay)
            {
                _elapsedTimeForDelay += Time.deltaTime;

                return;
            }

            // 寝言の透明度を上昇
            tmpSpriteColor.a = Mathf.Clamp(tmpSpriteColor.a + deltaFade, minTransparency, maxTransparency);
            tmpTextColor.a   = Mathf.Clamp(tmpTextColor.a   + deltaFade, minTransparency, maxTransparency);
        }
        else
        {
            _isEntered           = false;
            _elapsedTimeForDelay = 0;

            // 寝言の透明度を減少
            tmpSpriteColor.a = Mathf.Clamp(tmpSpriteColor.a - deltaFade, minTransparency, maxTransparency);
            tmpTextColor.a   = Mathf.Clamp(tmpTextColor.a   - deltaFade, minTransparency, maxTransparency);
        }

        // 寝言の透明度を反映
        spriteRenderer.color = tmpSpriteColor;
        textMesh.color       = tmpTextColor;
    }
}
