using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class NegotoController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTrf;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private InventoryContainerBase largePlateContainer;

    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField]
    private TMP_Text text;

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

    // 寝言がアクティブか否か

    // 寝言のインデックス番号
    public int selfIndex;

    // マネージャーのインスタンス
    public NegotoManager negotoManager;

    // レシピリストのコピー
    public List<RequireFoods> recipe;

    // プレイヤーが表示範囲に入ったか否か
    private bool _isEntered;

    // 寝言表示のフェードインの遅延量
    private float _fadeInDelay;

    // プレイヤーが寝言表示範囲に入ってからの経過時間
    private float _elapsedTimeForDelay;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        selfIndex     = -1;
        negotoManager = NegotoManager.Instance;
        recipe        = negotoManager.Recipe;
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // selfIndexが設定されたら初期化
        yield return new WaitWhile(() => selfIndex == -1);

        // 寝言データベースに登録があったらそちらを反映
        if (negotoManager.NegotoData.Entries.Any(e => e.SelfIndex == selfIndex))
        {
            var selfDatabaseEntry = negotoManager.NegotoData.Entries.First(e => e.SelfIndex == selfIndex);
            requireFood   = selfDatabaseEntry.RequireFood;
            requireStates = selfDatabaseEntry.RequireStates;
            IsActive      = selfDatabaseEntry.IsActive;
            textMesh.text = requireFood.ItemName;
        }
        // なければ初期化
        else
        {
            IsActive = true;
            // 初期表示のアイテムをランダムに選定
            UpdateContent();
        }
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
        // 寝言が有効じゃなければ表示しない
        if (!IsActive) return;

        var tmpSpriteColor = spriteRenderer.color;
        var tmpTextColor   = textMesh.color;

        // 寝言表示範囲にプレイヤーがいれば表示
        if (negotoManager.IsPlayerNeared)
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

    /// <summary>
    /// 寝言の内容を更新する
    /// </summary>
    public void UpdateContent()
    {
        // さらに表示するレシピ材料がなければ自身を無効化して終了
        if (recipe.Count == 0)
        {
            requireFood   = null;
            requireStates = null;
            textMesh.text = null;
            IsActive      = false;

            UpdateNegotoDatabase();

            return;
        }

        var foodIndex = Random.Range(0, recipe.Count);
        requireFood   = recipe[foodIndex].Food;
        requireStates = recipe[foodIndex].States;
        // TODO: 必須状態までを含む文字列を表示するようにし、さらに一部をぼかす
        textMesh.text = requireFood.ItemName;

        UpdateNegotoDatabase();

        recipe.RemoveAt(foodIndex);
    }

    /// <summary>
    /// アクティブか否かを設定し、データベースを更新する
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        IsActive = isActive;

        UpdateNegotoDatabase();
    }

    /// <summary>
    /// 寝言データベースを現在の内容で更新する
    /// </summary>
    private void UpdateNegotoDatabase()
    {
        // 寝言情報がデーターベースになければ追加
        if (negotoManager.NegotoData.Entries.All(e => e.SelfIndex != selfIndex))
        {
            negotoManager.NegotoData.AddEntry(selfIndex, requireFood, requireStates, IsActive);
        }
        // すでにある場合は上書き更新
        else
        {
            negotoManager.NegotoData.UpdateEntry(selfIndex, requireFood, requireStates, IsActive);
        }
    }
}
