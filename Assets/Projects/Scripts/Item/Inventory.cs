using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    // 現在選択しているアイテム
    protected Item _selectedItem;
    // 最後に選択していたアイテムのインデックス
    private int _lastSelectedIndex = 0;
    // スロットが選択可能状態か否か
    private bool _isSlotEnabled = true;
    // 交換時の対象アイテム
    private Item _itemToSwap;
    // インベントリ内のすべてのアイテム
    public Item[] allItems;

    // インベントリが持つスロットの数
    [SerializeField]
    protected int _slotSize;

    public Item SelectedItem { get => _selectedItem; set => _selectedItem = value; }
    public int LastSelectedIndex { get => _lastSelectedIndex; set => _lastSelectedIndex = value; }
    public bool IsSlotEnabled { get => _isSlotEnabled; protected set => _isSlotEnabled = value; }
    public Item ItemToSwap { get => _itemToSwap; set => _itemToSwap = value; }
    public Item[] AllItems { get => allItems; protected set => allItems = value; }
    public int SlotSize { get => _slotSize; }

    /// <summary>
    /// 変数として保持したラストインデックスのスロットに選択を戻す
    /// 引数がない場合はラストインデックスを使用
    /// </summary>
    /// <param name="index">選択するインデックス</param>
    public abstract void SelectSlot(int index);

    /// <summary>
    /// 全スロットを選択不可にする
    /// </summary>
    public abstract void DisableAllSlot();

    /// <summary>
    /// 全スロットを選択可能にする
    /// </summary>
    public abstract void EnableAllSlot();
}
