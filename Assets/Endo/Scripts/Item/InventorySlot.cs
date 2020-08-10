using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour, ISelectHandler
{
    protected GameObject _selfInvObj;
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
    /// <param name="index">配置させるインベントリスロットのインデックス</param>
    public void SetItem(Item item, InventoryManager inv = null, int index = -1)
    {
        selfItem = item;

        if (item != null)
        {
            itemName.text = item.ItemName;
        }

        // 初期化時じゃなければアイテム配列更新
        if (inv != null)
        {
            if (inv is CookingInventoryBase) return;
            int selfIndex = (inv is PlayerInventory)
                ? inv.lastSelectedIndex
                : GetSelfIndex(_selfInvSlotObjs, gameObject);

            inv.AllItems[index != -1 ? index : selfIndex] = selfItem;

            // プレイヤーInvならコンテナ更新（暫定）
            if (inv is PlayerInventory)
            {
                (inv as PlayerInventory).container.UpdateItem(index != -1 ? index : selfIndex, selfItem, FoodState.Raw);
            }
            //else if(inv is CookingInventoryBase)
            //{
            //    (inv as CookingInventoryBase).container.UpdateItem(selfIndex, selfItem, FoodState.Raw);
            //}
        }
    }

    /// <summary>
    /// スロットアイテムを削除する
    /// </summary>
    public void RemoveItem(InventoryManager inv)
    {
        int selfIndex           = GetSelfIndex(_selfInvSlotObjs, gameObject);
        selfItem                = null;
        itemName.text           = "";
        inv.AllItems[selfIndex] = selfItem;

        // プレイヤーInvならSOも更新
        if (inv is PlayerInventory)
        {
            (inv as PlayerInventory).container.RemoveItem(selfIndex);
        }
    }

    public Item GetInAllItem(InventoryManager inv)
    {
        return inv.AllItems[GetSelfIndex(_selfInvSlotObjs, gameObject)];
    }

    //焼き処理の後空いているインベントリに焼いた○○として表示させる。
    public void ChangeFoodName(InventoryManager inv, int index)
    {
        if (inv.AllItems[index] == null) return;
        itemName.text = "焼いた" + inv.AllItems[index].ItemName;
        //Debug.Log(inv.AllItems[GetSelfIndex(_selfInvSlotObjs, gameObject)].ItemName);
    }
    public void CutFoodName(InventoryManager inv, int index)
    {
        if (inv.AllItems[index] == null) return;
        itemName.text = "切れた" + inv.AllItems[index].ItemName;
        //Debug.Log(inv.AllItems[GetSelfIndex(_selfInvSlotObjs, gameObject)].ItemName);
    }
    public void BoilFoodName(InventoryManager inv, int index)
    {
        if (inv.AllItems[index] == null) return;
        itemName.text = "煮込んだ" + inv.AllItems[index].ItemName;
        //Debug.Log(inv.AllItems[GetSelfIndex(_selfInvSlotObjs, gameObject)].ItemName);
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

        // スロット配列が未生成なら弾く
        if (objToSearch == null) return result;

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
