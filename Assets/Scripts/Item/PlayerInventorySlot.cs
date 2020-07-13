using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventorySlot : InventorySlot
{
    void Start()
    {
        _invSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");

        // インベントリスロットを選択
        EventSystem.current.SetSelectedGameObject(_invSlotObjs[0]);
    }

    /// <summary>
    /// 選択スロット変更時に選択アイテムをSelfItemに渡し、インデックスを保持
    /// </summary>
    /// <param name="_"></param>
    public override void OnSelect(BaseEventData _)
    {
        PlayerInventory.Instance.selectedItem = SelfItem;

        for (int i = 0; i < _invSlotObjs.Length; i++)
        {
            if (_invSlotObjs[i] == gameObject)
            {
                PlayerInventory.Instance.lastSelectedIndex = i;
            }
        }
    }

    /// <summary>
    /// 選択しているアイテムが使用可能なら使用する
    /// 交換モードがtrueなら、受け渡しを行う
    /// </summary>
    public override void OnClick()
    {
        GameObject[]              _refInvSlotObjs;
        RefrigeratorInventorySlot _refInvSlot;

        // 選択モードなら
        if (PlayerInventory.Instance.isSwapMode)
        {
            // 冷蔵庫Inv取得
            _refInvSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");

            // 選択スロットを交換アイテムに指定
            PlayerInventory.Instance.itemToSwap = SelfItem;

            // 選択スロットに交換先のアイテムを配置
            SelfItem      = RefrigeratorInventory.Instance.itemToSwap;
            ItemName.text = RefrigeratorInventory.Instance.itemToSwap.ItemName;

            // 交換先に指定アイテムを渡す
            _refInvSlot               = _refInvSlotObjs[RefrigeratorInventory.Instance.indexToSwap].GetComponent<RefrigeratorInventorySlot>();
            _refInvSlot.SelfItem      = PlayerInventory.Instance.itemToSwap;
            _refInvSlot.ItemName.text = PlayerInventory.Instance.itemToSwap.ItemName;

            // フォーカスを冷蔵庫に戻す
            PlayerInventory.Instance.DisableAllSlot();
            RefrigeratorInventory.Instance.EnableAllSlot();
            RefrigeratorInventory.Instance.SelectSlot(RefrigeratorInventory.Instance.indexToSwap);

            // 一時保存のアイテムをクリア
            PlayerInventory.Instance.itemToSwap = null;
            RefrigeratorInventory.Instance.itemToSwap    = null;
            RefrigeratorInventory.Instance.indexToSwap   = -1;
        }
        else
        {
            UsableItem selectedItem = PlayerInventory.Instance.selectedItem as UsableItem;

            if (selectedItem != null && selectedItem.IsUsable)
            {
                selectedItem.Use();
            }
        }
    }
}
