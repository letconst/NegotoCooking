using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventorySlot : InventorySlot, ISelectHandler
{
    // スロット内のアイテム
    private Item _selfItem;

    private Item SelfItem { get => _selfItem; set => _selfItem = value; }

    void Start()
    {
        _invObj   = GameObject.FindGameObjectWithTag("PlayerInventory");
        _invSlots = GameObject.FindGameObjectsWithTag("PlayerInventorySlot");

        _inv = _invObj.GetComponent<Inventory>();

        // インベントリスロットを選択
        EventSystem.current.SetSelectedGameObject(_invSlots[0]);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 選択変更時に選択アイテムを渡す
        _inv.SelectedItem = SelfItem;
    }

    /// <summary>
    /// スロットにアイテムを渡し、アイテム名を表示させる
    /// </summary>
    /// <param name="item">配置および表示させるアイテム</param>
    public void SetItem(Item item)
    {
        SelfItem = item;

        if (item != null)
        {
            ItemName.text = item.ItemName;
        }
    }
}
