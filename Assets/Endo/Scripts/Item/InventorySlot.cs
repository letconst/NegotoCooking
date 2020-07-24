using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour, ISelectHandler
{
    // インベントリオブジェクト
    protected GameObject _selfInvObj;
    // インベントリスロットオブジェクト
    protected GameObject[] _selfInvSlotObjs;
    // スロット内のアイテム
    public Item selfItem;

    // アイテム名
    [SerializeField]
    public Text itemName;

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
    /// インベントリインスタンスの指定があれば、当該アイテム配列も更新する
    /// </summary>
    /// <param name="item">配置および表示させるアイテム</param>
    /// <param name="inv">インベントリのインスタンス</param>
    public void SetItem(Item item, InventoryManager inv = null)
    {
        selfItem = item;

        if (item != null)
        {
            itemName.text = item.ItemName;
        }

        // 初期化時じゃなければアイテム配列更新
        if (inv != null)
        {
            inv.AllItems[GetSelfIndex(_selfInvSlotObjs, gameObject)] = selfItem;
        }
    }

    /// <summary>
    /// スロットアイテムを削除する
    /// </summary>
    public void RemoveItem()
    {
        selfItem = null;
        itemName.text = "";
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
