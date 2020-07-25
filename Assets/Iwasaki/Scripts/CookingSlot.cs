using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingSlot : InventorySlotGrid
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerInventory.Instance.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject slotObj = Instantiate(_slotPrefab, transform);
            PlayerInventorySlot slot = slotObj.GetComponent<PlayerInventorySlot>();

            //slot.SetItem(null);

            // デバッグ用
            // 初期アイテムを持たせる
            // 使用する際はSlotWrapperのInvSlotGridコンポのAllItemsにアイテムを指定してあげる
            if (i < PlayerInventory.Instance.AllItems.Length)
            {
                // アイテムをスロットに配置
                slot.SetItem(PlayerInventory.Instance.AllItems[i]);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }
}