using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterController : SingletonMonoBehaviour<MasterController>
{
    private ChoicePopup _choicePopup;

    // 近くにいるか否か
    private bool _isNear;

    // 確認ウィンドウが表示中か否か
    private bool _isShowingWindow;

    // 確認ウィンドウのコルーチン
    private IEnumerator _windowCor;

    private void Start()
    {
        _choicePopup = GameObject.FindGameObjectWithTag("SelectWindow")
                                 .GetComponent<ChoicePopup>();
    }

    // Update is called once per frame
    private void Update()
    {
        // インタラクトで調理完了を行う
        if (_isNear                         && // インタラクト範囲内にいる
            !_isShowingWindow               && // 確認ウィンドウ表示中ではない
            Input.GetButtonDown("Interact") && // インタラクトボタン押下
            !PushPause.Instance.IsNowPausing)  // ポーズ中ではない
        {
            StartCoroutine(nameof(InputHandler));
        }
    }

    private IEnumerator InputHandler()
    {
        _windowCor       = _choicePopup.ShowWindow("師匠を起こしますか？");
        _isShowingWindow = true;

        // ボタン入力待機
        yield return _windowCor;

        // はい選択で判定実行
        if (_windowCor.Current != null &&
            (bool) _windowCor.Current)
        {
            // 調理判定
            Judgement();
            // Debug.Log($"FailCount: {GameManager.Instance.FailCount}");

            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.Result);
        }

        _choicePopup.HideWindow();
        _isShowingWindow = false;
        _windowCor       = null;
    }

    /// <summary>
    /// 調理の判定を行う
    /// </summary>
    public void Judgement()
    {
        var largePlateContainer = InventoryManager.Instance.LargePlateContainer;
        var recipeDB            = InventoryManager.Instance.RecipeDatabase;
        // TODO: 最終的にはランダムに選択されたレシピで処理する
        var targetRecipe = recipeDB.GetRecipeByName("エビのスープ");

        // 必須食材のコピー（チェックリスト用）
        var foodsToJudge = new List<RequireFoods>(targetRecipe.RequireFoods);

        //捨てるべからず
        if (GameManager.Instance.StatisticsManager.throwInCount >= 3)
        {
            GameManager.Instance.FailCount++;
        }

        //素早く丁寧に
        if (TimeCounter.CurrentTime < 100)
        {
            GameManager.Instance.FailCount++;
        }

        // 大皿の中身がレシピ通りかチェック
        foreach (var foodInPlate in largePlateContainer.Container)
        {
            foreach (var requireFood in foodsToJudge.ToList())
            {
                // 要件を満たす食材が大皿にあればチェックリストから当該食材を外す
                // TODO: 複数の状態が必要な場合への対応
                if (foodInPlate.Item == requireFood.Food                                &&
                    foodInPlate.States.All(state => requireFood.States.Contains(state)) &&
                    foodInPlate.States.Count == requireFood.States.Count)
                {
                    foodsToJudge.Remove(requireFood);
                }
            }
        }

        // 必須食材の要件を満たさないものが含まれていたら失敗
        // やり方間違えるべからず
        if (foodsToJudge.Count >= 3)
        {
            GameManager.Instance.FailCount++;
        }

        // すべての食材が間違っていたらさらに評価ダウン
        if (foodsToJudge.Count == targetRecipe.RequireFoods.Count)
        {
            GameManager.Instance.FailCount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _choicePopup.HideWindow();
        _isNear          = false;
        _isShowingWindow = false;

        // 確認ウィンドウ表示中だったら入力待機を解除して非表示に
        if (_windowCor == null) return;

        StopCoroutine(nameof(InputHandler));
        _windowCor = null;
    }
}
