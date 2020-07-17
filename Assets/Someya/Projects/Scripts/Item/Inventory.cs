using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 現在選択しているアイテム
    protected Item _selectedItem;
    // インベントリ内のすべてのアイテム
    public Item[] allItems;

    // インベントリが持つスロットの数
    [SerializeField]
    protected int _slotSize;

    public Item SelectedItem { get => _selectedItem; set => _selectedItem = value; }
    public Item[] AllItems { get => allItems; private set => allItems = value; }
    public int SlotSize { get => _slotSize; private set => _slotSize = value; }
}
