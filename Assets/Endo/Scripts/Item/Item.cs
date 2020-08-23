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

    // 食材3Dオブジェクト
    [SerializeField]
    private GameObject _foodObj;

    public KindOfItem KindOfItem1 => _kindOfItem;
    public Sprite     ItemIcon    => _itemIcon;
    public GameObject FoodObj     => _foodObj;
    public string     ItemName    => _itemName;
}
