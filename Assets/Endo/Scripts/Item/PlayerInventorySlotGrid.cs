﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySlotGrid : InventorySlotGrid
{
    private PlayerInventory _selfInv;

    // Start is called before the first frame update
    void Start()
    {
        _selfInvObj = GameObject.FindGameObjectWithTag("PlayerInventory");
        _selfInv    = _selfInvObj.GetComponent<PlayerInventory>();

        for (int i = 0; i < _selfInv.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject          slotObj = Instantiate(_slotPrefab, transform);
            PlayerInventorySlot slot    = slotObj.GetComponent<PlayerInventorySlot>();

            //slot.SetItem(null);

            // デバッグ用
            // 初期アイテムを持たせる
            // 使用する際はSlotWrapperのInvSlotGridコンポのAllItemsにアイテムを指定してあげる
            if (i < _selfInv.AllItems.Length)
            {
                // アイテムをスロットに配置
                slot.SetItem(_selfInv.AllItems[i]);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }
}
