﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Refrigerator Inventorys", menuName = "Inventory/Create Refrigerator Inventorys")]
public class RefrigeratorInventoryContainers : ScriptableObject
{
    [SerializeField]
    private List<RefrigeratorInventoryContainerBase> _refInvContainers = new List<RefrigeratorInventoryContainerBase>();

    public List<RefrigeratorInventoryContainerBase> RefInvContainers { get => _refInvContainers; private set => _refInvContainers = value; }

    /// <summary>
    /// 冷蔵庫コンテナを新規追加する
    /// </summary>
    /// <param name="refObjName">追加する冷蔵庫のオブジェクトの名前</param>
    public void AddContainer(string refObjName)
    {
        // 同じ名前のコンテナがあれば追加しない
        if (RefInvContainers != null &&
            RefInvContainers.Any(container => container.SelfObjName == refObjName))
        {
            return;
        }

        RefInvContainers?.Add(new RefrigeratorInventoryContainerBase(refObjName));
    }

    /// <summary>
    /// 指定したインスタンスIDの冷蔵庫オブジェクトのコンテナを返す
    /// 指定IDに一致するコンテナが見つからない場合はnullを返す
    /// </summary>
    /// <param name="refObjName">取得したいコンテナの冷蔵庫オブジェクトの名前</param>
    /// <returns>コンテナ || null</returns>
    public RefrigeratorInventoryContainerBase GetContainer(string refObjName)
    {
        return RefInvContainers.FirstOrDefault(container => container.SelfObjName == refObjName);
    }
}

[System.Serializable]
public class RefrigeratorInventoryContainerBase
{
    // TODO: 冷蔵庫判別にオブジェクト名を使用しているが、同一名称が使われる恐れがあるため、できれば他の方法に切り替えたい
    [SerializeField, Tooltip("個々の冷蔵庫オブジェクトの名前")]
    public string SelfObjName;

    [SerializeField]
    private List<InventorySlotBase> _container = new List<InventorySlotBase>();

    public List<InventorySlotBase> Container { get => _container; private set => _container = value; }

    public RefrigeratorInventoryContainerBase(string objName)
    {
        SelfObjName = objName;
    }

    /// <summary>
    /// インベントリにアイテムを追加する
    /// </summary>
    /// <param name="item">追加するアイテム</param>
    /// <param name="state">アイテムの状態</param>
    public void AddItem(Item item, FoodState state = FoodState.None)
    {
        // スロットサイズを超過する場合は追加しない
        if (Container.Count >= RefrigeratorManager.Instance.SlotSize) return;

        Container.Add(new InventorySlotBase(item, state));
    }

    /// <summary>
    /// 指定したインデックスのスロットアイテムを取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns></returns>
    public Item GetItem(int index)
    {
        return Container[index].Item;
    }

    /// <summary>
    /// 指定したインデックスのスロットアイテムの状態を取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns></returns>
    public FoodState GetState(int index)
    {
        return Container[index].State;
    }

    /// <summary>
    /// 指定したインデックスのスロットのアイテムを更新する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <param name="item">更新アイテム</param>
    /// <param name="state">アイテムの状態</param>
    public void UpdateItem(int index, Item item, FoodState state)
    {
        if (Container.Count < index)
        {
            Debug.LogError("UpdateItem: Out of range");
            return;
        }

        Container[index].UpdateSlot(item, state);
    }

    /// <summary>
    /// 指定したインデックスのスロットにあるアイテムを削除する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    public void RemoveItem(int index)
    {
        if (Container.Count < index)
        {
            Debug.LogError("RemoveItem: Out of range");
            return;
        }

        Container[index].UpdateSlot(null);
    }
}
