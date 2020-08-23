using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : SingletonMonoBehaviour<MasterController>
{
    // 近くにいるか否か
    private bool _isNear;

    // 食材がすべて調理できているか否か
    private bool _isComplete = true;

    private InventoryContainerBase _largePlateContainer;

    // Update is called once per frame
    private void Update()
    {
        if (_largePlateContainer == null)
        {
            _largePlateContainer = InventoryManager.Instance.LargePlateContainer;
        }

        // X押下および接近時に調理完了を行う
        if (_isNear && (Input.GetKeyDown("joystick button 2") ||
                        Input.GetKeyDown(KeyCode.E)))
        {
            // 調理判定
            Judgement();

            // 大皿に食材があり、すべて調理済みならゲームクリア（仮）
            SceneChanger.Instance.SceneLoad((_isComplete)
                ? SceneChanger.SceneName.GameClear
                : SceneChanger.SceneName.GameOverScenes);
        }
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

        // 大皿の中身がレシピ通りかチェック
        foreach (var foodInPlate in _largePlateContainer.Container)
        {
            foreach (var requireFood in foodsToJudge.ToList())
            {
                foreach (var requireState in requireFood.States)
                {
                    // 要件を満たす食材が大皿にあればチェックリストから当該食材を外す
                    // TODO: 複数の状態が必要な場合への対応
                    if (foodInPlate.Item == requireFood.Food &&
                        foodInPlate.State == requireState)
                    {
                        foodsToJudge.Remove(requireFood);
                    }
                }

            }
        }

        // 必須食材の要件を満たさないものが含まれていたら失敗
        if (foodsToJudge.Count != 0) _isComplete = false;
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
