using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
    public enum KindOfItem
    {
        Seasoning,  // 調味料
        Food,       // 食材
        CookingTool // 調理器具
    }

    // アイテムの種類
    [SerializeField]
    private KindOfItem _kindOfItem;

    // アイテムアイコン
    [SerializeField]
    private Sprite _itemIcon;

    // アイテム名
    [SerializeField]
    private string _itemName;

    // アイテム説明
    [SerializeField]
    private string _itemDesc;

    // 初期装備か否か
    [SerializeField]
    private bool _isDefault = false;

    // 使用可能か否か
    [SerializeField]
    private bool _isUsable = false;

    public KindOfItem KindOfItem1 { get => _kindOfItem; }
    public Sprite     ItemIcon    { get => _itemIcon; }
    public string     ItemName    { get => _itemName; }
    public string     ItemDesc    { get => _itemDesc; }
    public bool       IsDefault   { get => _isDefault; }
    public bool       IsUsable    { get => _isUsable; }
}
