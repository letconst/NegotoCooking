using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotFunctions : MonoBehaviour
{
    private GameObject                         _nearRefObj;
    private GameObject                         _refInvObj;
    private TmpInventoryManager                _invManager;
    private PlayerInventoryContainer           _playerContainer;
    private RefrigeratorInventoryContainerBase _nearRefContainer;
    private InventoryRenderer                  _playerInvRenderer;
    private InventoryRenderer                  _refInvRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _nearRefObj        = RefrigeratorManager.Instance.currentNearObj;
        _refInvObj         = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
        _invManager        = TmpInventoryManager.Instance;
        _playerContainer   = _invManager.PlayerContainer;
        _playerInvRenderer = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<InventoryRenderer>();
    }

    // Update is called once per frame
    void Update()
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

    public void OnClickForPlayer()
    {
        _nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;

        // 交換モードなら
        if (_invManager.isSwapMode)
        {
            // 交換先にアイテムを渡す
            _nearRefContainer.UpdateItem(_refInvRenderer.LastSelectedIndex,
                                         _playerContainer.GetItem(GetSelfIndex()),
                                         _playerContainer.GetState(GetSelfIndex()));

            // 選択スロットに交換先のアイテムを配置
            _playerContainer.UpdateItem(GetSelfIndex(),
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

    public void OnClickForRefrigerator()
    {
        _nearRefContainer = RefrigeratorManager.Instance.NearRefrigeratorContainer;
        InventorySlotBase nearRefSlot      = _nearRefContainer.Container[GetSelfIndex()];
        Item              selfItem         = nearRefSlot.Item;
        FoodState         selfState        = nearRefSlot.State;

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
                _nearRefContainer.RemoveItem(GetSelfIndex());

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
}
