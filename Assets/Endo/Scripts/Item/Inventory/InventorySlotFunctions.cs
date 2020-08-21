using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventorySlotFunctions : MonoBehaviour
{
    private GameObject                         _refInvObj;
    private TmpInventoryManager                _invManager;
    private PlayerInventoryContainer           _playerContainer;
    private RefrigeratorInventoryContainerBase _nearRefContainer;
    private InventoryRenderer                  _playerInvRenderer;
    private InventoryRenderer                  _refInvRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _refInvObj         = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _invManager        = TmpInventoryManager.Instance;
        _playerContainer   = _invManager.playerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // メインシーンのときに、冷蔵庫インベントリオブジェが未取得なら取得を試みる
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (_refInvObj == null)
            {
                _refInvObj = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
            }
            else if (_refInvRenderer == null)
            {
                _refInvRenderer = _refInvObj.GetComponent<InventoryRenderer>();
            }
        }
    }

    /// <summary>
    /// 同じ階層のオブジェクトから自身を探し、見つかったインデックスを返す
    /// 返り値が-1なら該当なし
    /// </summary>
    /// <returns>インデックス</returns>
    public int GetSelfIndex()
    {
        int result = -1;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject == gameObject)
            {
                result = i;
            }
        }

        return result;
    }

    /// <summary>
    /// プレイヤーインベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForPlayer()
    {
        _nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;

        // 交換モードなら
        if (_invManager.isSwapMode)
        {
            int selfIndex = GetSelfIndex();

            // 交換先にアイテムを渡す
            _nearRefContainer.UpdateItem(_refInvRenderer.LastSelectedIndex,
                                         _playerContainer.GetItem(selfIndex),
                                         _playerContainer.GetState(selfIndex));

            // 選択スロットに交換先のアイテムを配置
            _playerContainer.UpdateItem(selfIndex,
                                        _invManager.itemToSwapFromRef,
                                        _invManager.itemStateToSwap);

            // フォーカスを冷蔵庫に戻す
            _playerInvRenderer.DisableAllSlot();
            _refInvRenderer.EnableAllSlot();
            _refInvRenderer.SelectSlot();

            // キャッシュアイテムクリア
            _invManager.itemToSwapFromRef = null;
            _invManager.itemStateToSwap   = FoodState.None;
        }
    }

    /// <summary>
    /// 冷蔵庫インベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForRefrigerator()
    {
        _nearRefContainer             = RefrigeratorManager.Instance.NearRefrigeratorContainer;
        int               selfIndex   = GetSelfIndex();
        InventorySlotBase nearRefSlot = _nearRefContainer.Container[selfIndex];
        Item              selfItem    = nearRefSlot.Item;
        FoodState         selfState   = nearRefSlot.State;

        // スロットにアイテムがなければ弾く
        if (selfItem == null) return;

        for (int i = 0; i < _playerContainer.Container.Count; i++)
        {
            // プレイヤーインベントリに空きがあったら
            if (_playerContainer.Container[i].Item == null)
            {
                // そのスロットにアイテムを配置
                _playerContainer.UpdateItem(i, selfItem, selfState);

                // 冷蔵庫スロットは空にする
                _nearRefContainer.RemoveItem(selfIndex);

                return;
            }
        }

        // プレイヤーとアイテム交換
        // 交換アイテムをキャッシュ
        _invManager.itemToSwapFromRef = selfItem;
        _invManager.itemStateToSwap   = selfState;
        // 交換モード発動
        _invManager.isSwapMode = true;
        // プレイヤースロットにフォーカス
        _refInvRenderer.DisableAllSlot();
        _playerInvRenderer.EnableAllSlot();
        _playerInvRenderer.SelectSlot();

        // ここから先はOnClickForPlayer()
    }

    /// <summary>
    /// 調理時のプレイヤーインベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForCooking()
    {
        //食料prefabの親
        GameObject foodParent = GameObject.FindGameObjectWithTag("FoodParent");

        GameObject foodPosition = GameObject.FindGameObjectWithTag("FoodPosition");
        Transform  foodTrf      = foodPosition.transform;

        int selfIndex = GetSelfIndex();

        PlayerInventoryContainer playerContainer = TmpInventoryManager.Instance.PlayerContainer;
        InventorySlotBase selfSlot               = playerContainer.Container[selfIndex];

        // 現在のシーン名
        string curSceneName = SceneManager.GetActiveScene().name;

        // 投入できない食材の状態
        FoodState disallowState = FoodState.None;

        switch (curSceneName)
        {
            case "BakeScenes":
                disallowState = FoodState.Cooked;
                break;

            case "BoilScenes":
                disallowState = FoodState.Boil;
                break;

            case "CutScenes":
                disallowState = FoodState.Cut;
                break;

            default:
                Debug.LogWarning($"シーン「{curSceneName}」の投入不可状態の指定がありません");
                break;
        }

        // 条件に満たない食材は投入できない
        if (!FireControl.clickBool          ||
            !CutGauge.clickBool             ||
            selfSlot.Item == null           ||
            selfSlot.State == disallowState ||
            selfSlot.Item.KindOfItem1 == Item.KindOfItem.Seasoning) return;

        if (curSceneName == "CutScenes")
        {
            CutGauge.clickBool = false;
        }
        else
        {
            FireControl.clickBool = false;
        }

        // 食材を表示
        GameObject foodChild = Instantiate(selfSlot.Item.FoodObj,
                                           foodTrf.position,
                                           new Quaternion(foodTrf.rotation.x,
                                                          foodTrf.rotation.y,
                                                          foodTrf.rotation.z,
                                                          foodTrf.rotation.w));
        foodChild.transform.parent = foodParent.transform;

        switch (curSceneName)
        {
            case "BakeScenes":
                BakeController.foodBeingBaked = selfSlot.Item;
                break;

            case "BoilScenes":
                BoilController.foodBeingBoiled = selfSlot.Item;
                break;

            case "CutScenes":
                CutController.foodBeingCut = selfSlot.Item;
                break;

            default:
                Debug.LogWarning($"シーン「{curSceneName}」の調理中食材投入先の指定がありません");
                break;
        }

        // プレイヤーコンテナから投入アイテムを削除
        playerContainer.RemoveItem(selfIndex);

        // 投入元のスロットインデックスを記憶
        TmpInventoryManager.Instance.puttedSlotIndex = selfIndex;
    }
}
