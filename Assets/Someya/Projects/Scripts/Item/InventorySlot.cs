using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // インベントリオブジェクト
    protected GameObject _invObj;
    // インベントリスロットオブジェクト
    protected GameObject[] _invSlots;
    // インベントリのインスタンス
    protected Inventory _inv;

    // アイテム名
    [SerializeField]
    Text _itemName;

    public Text ItemName { get => _itemName; set => _itemName = value; }
}
