using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotGrid : MonoBehaviour
{
    // インベントリオブジェクト
    protected GameObject _selfInvObj;

    // 
    [SerializeField]
    protected GameObject _slotPrefab;

    /// <summary>
    /// インベントリから指定した名前のアイテムを探す
    /// </summary>
    /// <param name="allItems">検索対象の配列</param>
    /// <param name="name">検索するアイテム名</param>
    /// <returns>
    /// 名称一致したアイテム
    /// </returns>
    public Item findItemByName(Item[] allItems, string name)
    {
        Item result = null;

        foreach (Item i in allItems)
        {
            if (i.ItemName == name)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
