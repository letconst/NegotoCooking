using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterController : SingletonMonoBehaviour<MasterController>
{
    // 近くにいるか否か
    private bool _isNear;

    // 食材がすべて調理できているか否か
    private bool                   _isComplete = true;
    private InventoryContainerBase _largePlateContainer;
    private ChoicePopup            _choicePopup;

    private void Start()
    {
        _choicePopup = GameObject.FindGameObjectWithTag("SelectWindow").GetComponent<ChoicePopup>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_largePlateContainer == null)
        {
            _largePlateContainer = InventoryManager.Instance.LargePlateContainer;
        }

        // インタラクトで調理完了を行う
        if (_isNear                         && // インタラクト範囲内にいる
            Input.GetButtonDown("Interact") && // インタラクトボタン押下
            Time.timeScale.Equals(1))          // ポーズ中ではない
        {
            StartCoroutine(nameof(InputHandler));
        }
    }

    private IEnumerator InputHandler()
    {
        var coroutine = _choicePopup.showWindow("師匠を起こしますか？");

        // ボタン入力待機
        yield return coroutine;

        // はい選択で判定実行
        if (coroutine.Current != null &&
            (bool) coroutine.Current)
        {
            // 調理判定
            Judgement();
            Debug.Log(GameManager.Instance.FailCount);

            SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.Result);
        }

        _choicePopup.HideWindow();
    }

    /// <summary>
    /// 調理の判定を行う
    /// </summary>
    private void Judgement()
    {
        var recipeDB = InventoryManager.Instance.RecipeDatabase;
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
        foreach (var foodInPlate in _largePlateContainer.Container)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isNear = false;
        _choicePopup.HideWindow();
    }
}
