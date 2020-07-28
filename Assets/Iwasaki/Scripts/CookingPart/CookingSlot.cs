using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingSlot : InventorySlotGrid
{
    private CookingInventoryBase _selfInv;

    // Start is called before the first frame update
    void Start()
    {
        _selfInvObj = GameObject.FindGameObjectWithTag("CookingInventory");
        _selfInv = _selfInvObj.GetComponent<CookingInventoryBase>();

        for (int i = 0; i < _selfInv.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject slotObj = Instantiate(_slotPrefab, transform);
            BakeCooking slot = slotObj.GetComponent<BakeCooking>();

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