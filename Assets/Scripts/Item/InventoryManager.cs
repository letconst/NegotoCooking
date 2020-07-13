using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryManager<T> : SingletonMonoBehaviour<T> where T : InventoryManager<T>
{
    // 現在選択しているアイテム
    [System.NonSerialized]
    public Item selectedItem;
    // 最後に選択していたアイテムのインデックス
    [System.NonSerialized]
    public int lastSelectedIndex = 0;
    // スロットが選択可能状態か否か
    private bool _isSlotEnabled = true;
    // 交換時の対象アイテム
    [System.NonSerialized]
    public Item itemToSwap;
    // インベントリ内のすべてのアイテム
    [SerializeField]
    private Item[] _allItems;

    // インベントリが持つスロットの数
    [SerializeField]
    protected int _slotSize;

    public bool IsSlotEnabled { get => _isSlotEnabled; protected set => _isSlotEnabled = value; }
    public Item[] AllItems { get => _allItems; protected set => _allItems = value; }
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
