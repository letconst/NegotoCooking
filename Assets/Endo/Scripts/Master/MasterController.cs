using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : SingletonMonoBehaviour<MasterController>
{
    // 調理済か否か（デバッグ用、本来は調理パートに持たせる）
    [SerializeField]
    private bool _isCooked = false;

    // 近くにいるか否か
    private bool _isNear = false;

    // 食材がすべて調理できているか否か
    private bool _isComplete = true;

    public InventoryContainerBase largePlateContainer;

    // Update is called once per frame
    void Update()
    {
        // X押下および接近時に調理完了を行う
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.E)) && _isNear)
        {
            // 大皿のすべての食材が調理できているかチェック
            for (int i = 0; i < largePlateContainer.Container.Count; i++)
            {
                if (largePlateContainer.Container[i].State != FoodState.Cooked)
                {
                    _isComplete = false;
                }
            }

            // 大皿に食材があり、すべて調理済みならゲームクリア（仮）
            if (_isComplete && largePlateContainer.Container.Count != 0)
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
    /// 接近時にフラグを立てる
    /// </summary>
    private void OnTriggerEnter()
    {
        _isNear = true;
    }

    /// <summary>
    /// 接近時にフラグを消す
    /// </summary>
    private void OnTriggerExit()
    {
        _isNear = false;
    }
}
