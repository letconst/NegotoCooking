using System.Collections;
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
            if (_selfInv.container.Container.Count <= i)
            {
                if (i < _selfInv.AllItems.Length)
                {
                    // アイテムをスロットに配置
                    _selfInv.container.AddItem(_selfInv.AllItems[i], FoodState.Raw);
                    slot.SetItem(_selfInv.AllItems[i]);
                }
                else
                {
                    _selfInv.container.AddItem(null, FoodState.Raw);
                    slot.SetItem(null);
                }
            }
            else
            {
                slot.SetItem(_selfInv.container.Container[i].Item, _selfInv);
                _selfInv.AllItems[i] = _selfInv.container.Container[i].Item;
                switch (_selfInv.container.Container[i].State)
                {
                    case FoodState.Cooked:
                        slot.itemName.text = "焼いた" + _selfInv.container.Container[i].Item.ItemName;
                        break;

                    case FoodState.Burnt:
                        slot.itemName.text = "焦げた" + _selfInv.container.Container[i].Item.ItemName;
                        break;
                }
            }            
        }
    }

    private void OnApplicationQuit()
    {
        _selfInv.container.Container.Clear();
    }
}
