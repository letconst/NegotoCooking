using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventorySlot : InventorySlot
{
    private PlayerInventory _selfInv;

    void Start()
    {
        _selfInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");
        _selfInvSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");

        _selfInv = _selfInvObj.GetComponent<PlayerInventory>();

        // インベントリスロットを選択
        EventSystem.current.SetSelectedGameObject(_selfInvSlotObjs[0]);
    }

    /// <summary>
    /// 選択スロット変更時に選択アイテムをSelfItemに渡し、インデックスを保持
    /// </summary>
    /// <param name="_"></param>
    //public override void OnSelect(BaseEventData _)
    //{
    //    _selfInv.selectedItem = selfItem;

    //    for (int i = 0; i < _selfInvSlotObjs.Length; i++)
    //    {
    //        if (_selfInvSlotObjs[i] == gameObject)
    //        {
    //            _selfInv.lastSelectedIndex = i;
    //        }
    //    }
    //}

    /// <summary>
    /// 選択しているアイテムが使用可能なら使用する
    /// 交換モードがtrueなら、受け渡しを行う
    /// </summary>
    public override void OnClick()
    {
        // 交換モードなら
        if (_selfInv.isSwapMode)
        {
            GameObject[]              _refInvSlotObjs;
            RefrigeratorInventory     _refInv = RefrigeratorManager.Instance.currentNearObj.GetComponentInChildren<RefrigeratorInventory>();
            RefrigeratorInventorySlot _refInvSlot;
            int selfIndex = GetSelfIndex(_selfInvSlotObjs, gameObject);

            // 冷蔵庫Inv取得
            _refInvSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");

            // 選択スロットを交換アイテムに指定
            _selfInv.itemToSwap = selfItem;

            // 選択スロットに交換先のアイテムを配置
            SetItem(_refInv.itemToSwap, _selfInv);

            // 交換先に指定アイテムを渡す
            _refInvSlot = _refInvSlotObjs[_refInv.indexToSwap].GetComponent<RefrigeratorInventorySlot>();
            _refInvSlot.SetItem(_selfInv.itemToSwap, _refInv);

            // フォーカスを冷蔵庫に戻す
            _selfInv.DisableAllSlot();
            _refInv.EnableAllSlot();
            _refInv.SelectSlot(_refInv.indexToSwap);

            // 一時保存のアイテムをクリア
            _selfInv.itemToSwap = null;
            _refInv.itemToSwap  = null;
            _refInv.indexToSwap = -1;
        }
        else
        {
            // おそらく使わないため廃止予定
            Usable selectedItem = _selfInv.selectedItem as Usable;

            if (selectedItem != null && ((Item)selectedItem).IsUsable)
            {
                selectedItem.Use();
            }
        }
    }
}
