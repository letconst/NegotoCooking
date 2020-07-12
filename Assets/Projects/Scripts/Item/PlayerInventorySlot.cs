using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventorySlot : InventorySlot
{
    private GameObject                _refInvObj;
    private GameObject[]              _refInvSlotObjs;
    private PlayerInventory           _playerInv;
    private RefrigeratorInventory     _refInv;
    private RefrigeratorInventorySlot _refInvSlot;

    void Start()
    {
        _invObj      = GameObject.FindGameObjectWithTag("PlayerInventory");
        _invSlotObjs = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");

        _playerInv = _invObj.GetComponent<PlayerInventory>();

        // インベントリスロットを選択
        EventSystem.current.SetSelectedGameObject(_invSlotObjs[0]);
    }

    /// <summary>
    /// 選択スロット変更時に選択アイテムをSelfItemに渡し、インデックスを保持
    /// </summary>
    /// <param name="_"></param>
    public override void OnSelect(BaseEventData _)
    {
        _playerInv.SelectedItem = SelfItem;

        for (int i = 0; i < _invSlotObjs.Length; i++)
        {
            if (_invSlotObjs[i] == gameObject)
            {
                _playerInv.LastSelectedIndex = i;
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
        if (_playerInv.IsSwapMode)
        {
            // 冷蔵庫Inv取得
            _refInvObj      = GameObject.FindGameObjectWithTag("RefrigeratorInventory");
            _refInvSlotObjs = GameObject.FindGameObjectsWithTag("RefrigeratorInventorySlot");
            _refInv         = _refInvObj.GetComponent<RefrigeratorInventory>();

            // 選択スロットを交換アイテムに指定
            _playerInv.ItemToSwap = SelfItem;

            // 選択スロットに交換先のアイテムを配置
            SelfItem      = _refInv.ItemToSwap;
            ItemName.text = _refInv.ItemToSwap.ItemName;

            // 交換先に指定アイテムを渡す
            _refInvSlot               = _refInvSlotObjs[_refInv.IndexToSwap].GetComponent<RefrigeratorInventorySlot>();
            _refInvSlot.SelfItem      = _playerInv.ItemToSwap;
            _refInvSlot.ItemName.text = _playerInv.ItemToSwap.ItemName;

            // フォーカスを冷蔵庫に戻す
            _playerInv.DisableAllSlot();
            _refInv.EnableAllSlot();
            _refInv.SelectSlot(_refInv.IndexToSwap);

            // 一時保存のアイテムをクリア
            _playerInv.ItemToSwap = null;
            _refInv.ItemToSwap    = null;
            _refInv.IndexToSwap   = -1;
        }
        else
        {
            UsableItem selectedItem = _playerInv.SelectedItem as UsableItem;

            if (selectedItem != null && selectedItem.IsUsable)
            {
                selectedItem.Use();
            }
        }
    }
}
