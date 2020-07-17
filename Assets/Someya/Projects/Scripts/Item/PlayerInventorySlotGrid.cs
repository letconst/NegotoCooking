using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySlotGrid : InventorySlotGrid
{
    // プレイヤーインベントリのインスタンス
    private PlayerInventory _inv;

    // Start is called before the first frame update
    void Start()
    {
        _invObj = GameObject.FindGameObjectWithTag("PlayerInventory");
        _inv = _invObj.GetComponent<PlayerInventory>();

        for (int i = 0; i < _inv.SlotSize; i++)
        {
            // スロットのインスタンス
            GameObject slotObj = Instantiate(_slotPrefab, this.transform);
            PlayerInventorySlot slot = slotObj.GetComponent<PlayerInventorySlot>();

            //slot.SetItem(null);

            // デバッグ用
            // 初期アイテムを持たせる
            // 使用する際はSlotWrapperのInvSlotGridコンポのAllItemsにアイテムを指定してあげる
            if (i < _inv.AllItems.Length)
            {
                // アイテムをスロットに配置
                slot.SetItem(_inv.AllItems[i]);
            }
            else
            {
                slot.SetItem(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
