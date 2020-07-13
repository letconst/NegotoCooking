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
        Food        // 食材
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

    // 使用可能か否か
    [SerializeField]
    private bool _isUsable = false;

    public KindOfItem KindOfItem1 { get => _kindOfItem; }
    public Sprite     ItemIcon    { get => _itemIcon; }
    public string     ItemName    { get => _itemName; }
    public bool       IsUsable    { get => _isUsable; }
}

public interface Usable
{
    /// <summary>
    /// 所持アイテムを使用した際の処理
    /// </summary>
    void Use();
}
