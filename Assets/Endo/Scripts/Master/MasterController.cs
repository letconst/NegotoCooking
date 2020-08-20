using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : SingletonMonoBehaviour<MasterController>
{
    // 近くにいるか否か
    private bool _isNear = false;

    // 食材がすべて調理できているか否か
    private bool _isComplete = true;

    private InventoryContainerBase _largePlateContainer;

    // Update is called once per frame
    void Update()
    {
        if (_largePlateContainer == null)
        {
            _largePlateContainer = TmpInventoryManager.Instance.LargePlateContainer;
        }

        // X押下および接近時に調理完了を行う
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.E)) && _isNear)
        {
            // 調理判定
            Judgement();

            // 大皿に食材があり、すべて調理済みならゲームクリア（仮）
            if (_isComplete)
            {
                // ゲームクリアシーン読み込み
                SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameClear);
            }
            // 調理できていなければゲームオーバー
            else
            {
                SceneChanger.Instance.SceneLoad(SceneChanger.SceneName.GameOverScenes);
            }
        }
    }

    /// <summary>
    /// 調理の判定を行う
    /// </summary>
    private void Judgement()
    {
        RecipeDatabase recipeDB = TmpInventoryManager.Instance.RecipeDatabase;
        // TODO: 最終的にはランダムに選択されたレシピで処理する
        RecipeEntry targetRecipe = recipeDB.GetRecipeByName("エビのスープ");

        // 必須食材のコピー（チェックリスト用）
        List<RequireFoods> foodsToJudge = new List<RequireFoods>(targetRecipe.RequireFoods);

        // 大皿の中身がレシピ通りかチェック
        foreach (var foodInPlate in _largePlateContainer.Container)
        {
            foreach (var requireFood in foodsToJudge.ToList())
            {
                foreach (var state in requireFood.States)
                {
                    // 要件を満たす食材が大皿にあればチェックリストから当該食材を外す
                    if (foodInPlate.Item == requireFood.Food &&
                        foodInPlate.State == state)
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
