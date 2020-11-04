using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    private readonly Dictionary<FoodState, string> _foodStateName = new Dictionary<FoodState, string>()
    {
        {FoodState.None, "入れる"},
        {FoodState.Raw, ""},
        {FoodState.Cooked, "焼く"},
        {FoodState.Cut, "切る"},
        {FoodState.Boil, "煮る"}
    };

    /// <summary>
    /// 寝言がアクティブか否か
    /// </summary>
    public bool IsActive { get; private set; }

    private void Awake()
    {
        selfIndex     = -1;
        negotoManager = NegotoManager.Instance;
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // レシピデータが取得可能になるまで待機
        yield return new WaitWhile((() => negotoManager.Recipe == null));

        recipe = negotoManager.Recipe;

        // selfIndexが設定されたら初期化
        yield return new WaitWhile(() => selfIndex == -1);

        // 寝言データベースに登録があったらそちらを反映
        if (negotoManager.NegotoData.Entries.Any(e => e.SelfIndex == selfIndex))
        {
            var selfDatabaseEntry = negotoManager.NegotoData.Entries.First(e => e.SelfIndex == selfIndex);
            requireFood   = selfDatabaseEntry.RequireFood;
            requireStates = selfDatabaseEntry.RequireStates;
            IsActive      = selfDatabaseEntry.IsActive;
            textMesh.text = selfDatabaseEntry.ContentText;
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
        textMesh.text = BlurringText(requireFood, requireStates);

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
            negotoManager.NegotoData.AddEntry(selfIndex, requireFood, requireStates, textMesh.text, IsActive);
        }
        // すでにある場合は上書き更新
        else
        {
            negotoManager.NegotoData.UpdateEntry(selfIndex, requireFood, requireStates, textMesh.text, IsActive);
        }
    }

    /// <summary>
    /// 一部をぼかした寝言の内容を作成する
    /// </summary>
    /// <param name="targetFood">ぼかす対象の食材</param>
    /// <param name="targetStates">ぼかす対象の状態</param>
    /// <returns>ぼかした寝言</returns>
    private string BlurringText(Item targetFood, IEnumerable<FoodState> targetStates)
    {
        var result = Blurring(targetFood.ItemName) + "を";

        // resultに状態の名称を追記
        result = targetStates.Aggregate(result, (current, state) => current + Blurring(_foodStateName[state]));

        return result;

        string Blurring(string target)
        {
            var localResult = new StringBuilder(target);
            var tmpResult   = string.Copy(target);

            // 1文字ならぼかさず返す
            if (target.Length == 1) return localResult.ToString();

            foreach (var c in target.Select((v, i) => new
            {
                v,
                i
            }))
            {
                // 対象文字列が二文字であり、今回処理する文字が一文字目ならぼかさない（暫定）
                if (target.Length == 2 &&
                    c.i           == 0) continue;

                var rnd = Random.Range(0, 2);

                // 二文字ではない対象文字列の最後の文字の際、この文字より前の文字がすべてぼかされていなかったらこの文字はぼかす
                if (target.Length != 2                      &&
                    c.i           == localResult.Length - 1 &&
                    localResult.ToString().Substring(0, tmpResult.Length - 1).All(tmpC => tmpC != '…'))
                {
                    localResult[c.i] = '…';

                    break;
                }

                // 最後の文字の際、この文字より前の文字がすべてぼかされていたらこの文字は残す
                if (c.i == localResult.Length - 1 &&
                    localResult.ToString().Substring(0, tmpResult.Length - 1).All(tmpC => tmpC == '…'))
                {
                    break;
                }

                if (rnd != 1) continue;

                localResult[c.i] = '…';
            }

            return localResult.ToString();
        }
    }
}
