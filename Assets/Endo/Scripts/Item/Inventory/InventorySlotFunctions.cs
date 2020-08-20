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
        _playerContainer   = _invManager.PlayerContainer;
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
    /// 焼き調理時のプレイヤーインベントリスロットをクリックした際の動作
    /// </summary>
    public void OnClickForBakeAndBoil()
    {
        //食料prefabの親
        GameObject foodParent = GameObject.FindGameObjectWithTag("FoodParent");

        GameObject foodPosition = GameObject.FindGameObjectWithTag("FoodPosition");
        Transform  foodTrf      = foodPosition.transform;

        int selfIndex = GetSelfIndex();

        PlayerInventoryContainer playerContainer = TmpInventoryManager.Instance.PlayerContainer;
        InventorySlotBase selfSlot               = playerContainer.Container[selfIndex];

        // 条件に満たない食材は投入できない
        if (!FireControl.clickBool             ||
            selfSlot.Item == null              ||
            selfSlot.State == FoodState.Cooked ||
            selfSlot.Item.KindOfItem1 == Item.KindOfItem.Seasoning) return;

        FireControl.clickBool = false;

        // 食材を表示
        GameObject foodChild = Instantiate(selfSlot.Item.FoodObj,
                                           foodTrf.position,
                                           new Quaternion(foodTrf.rotation.x,
                                                          foodTrf.rotation.y,
                                                          foodTrf.rotation.z,
                                                          foodTrf.rotation.w));
        foodChild.transform.parent = foodParent.transform;
        FireControl.foodInProgress = selfSlot.Item;

        // プレイヤーコンテナから投入アイテムを削除
        playerContainer.RemoveItem(selfIndex);

        // 投入元のスロットインデックスを記憶
        TmpInventoryManager.Instance.puttedSlotIndex = selfIndex;
    }
}
