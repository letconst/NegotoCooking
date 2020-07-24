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
        // 選択モードなら
        if (PlayerInventory.Instance.isSwapMode)
        {
            GameObject[]              _refInvSlotObjs;
            RefrigeratorInventory     _refInv = RefrigeratorManager.Instance.currentNearObj.GetComponentInChildren<RefrigeratorInventory>();
            RefrigeratorInventorySlot _refInvSlot;

            // 冷蔵庫Inv取得
            _refInvSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");

            // 選択スロットを交換アイテムに指定
            PlayerInventory.Instance.itemToSwap = SelfItem;

            // 選択スロットに交換先のアイテムを配置
            SetItem(_refInv.itemToSwap);

            // 交換先に指定アイテムを渡す
            _refInvSlot = _refInvSlotObjs[_refInv.indexToSwap].GetComponent<RefrigeratorInventorySlot>();
            _refInvSlot.SetItem(PlayerInventory.Instance.itemToSwap);

            // フォーカスを冷蔵庫に戻す
            PlayerInventory.Instance.DisableAllSlot();
            _refInv.EnableAllSlot();
            _refInv.SelectSlot(_refInv.indexToSwap);

            // 一時保存のアイテムをクリア
            PlayerInventory.Instance.itemToSwap = null;
            _refInv.itemToSwap                  = null;
            _refInv.indexToSwap                 = -1;
        }
        else
        {
            Usable selectedItem = PlayerInventory.Instance.selectedItem as Usable;

            if (selectedItem != null && ((Item)selectedItem).IsUsable)
            {
                selectedItem.Use();
            }
        }
    }
}
