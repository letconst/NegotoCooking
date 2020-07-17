using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour, ISelectHandler
{
    // インベントリオブジェクト
    protected GameObject _invObj;
    // インベントリスロットオブジェクト
    protected GameObject[] _invSlotObjs;
    // スロット内のアイテム
    protected Item _selfItem;

    // アイテム名
    [SerializeField]
    Text _itemName;

    public Text ItemName { get => _itemName; set => _itemName = value; }
    public Item SelfItem { get => _selfItem; set => _selfItem = value; }

    /// <summary>
    /// 選択スロット変更時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnSelect(BaseEventData eventData) { }

    /// <summary>
    /// ボタンをクリックした際の処理
    /// </summary>
    public abstract void OnClick();

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

    /// <summary>
    /// スロットアイテムを削除する
    /// </summary>
    public void RemoveItem()
    {
        SelfItem = null;
        ItemName.text = "";
    }

    /// <summary>
    /// 渡されたスロット配列内から検索スロットを探し、見つかったらインデックスを返す
    /// 返り値が-1なら該当なし
    /// </summary>
    /// <param name="objToSearch">検索対象のスロット配列</param>
    /// <param name="selfObj">検索するスロット</param>
    /// <returns></returns>
    protected int GetSelfIndex(GameObject[] objToSearch, GameObject selfObj)
    {
        int result = -1;

        for (int i = 0; i < objToSearch.Length; i++)
        {
            if (objToSearch[i] == selfObj)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
