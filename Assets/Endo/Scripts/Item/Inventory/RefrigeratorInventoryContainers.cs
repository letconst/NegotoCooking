using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Refrigerator Inventories", menuName = "Inventory/Create Refrigerator Inventories")]
public class RefrigeratorInventoryContainers : ScriptableObject
{
    [SerializeField]
    private List<RefrigeratorInventoryContainerBase> refInvContainers = new List<RefrigeratorInventoryContainerBase>();

    public List<RefrigeratorInventoryContainerBase> RefInvContainers => refInvContainers;

    /// <summary>
    /// 冷蔵庫コンテナを新規追加する
    /// </summary>
    /// <param name="refObjName">追加する冷蔵庫のオブジェクトの名前</param>
    public void AddContainer(string refObjName)
    {
        // 同じ名前のコンテナがあれば追加しない
        if (RefInvContainers != null &&
            RefInvContainers.Any(container => container.selfObjName == refObjName))
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
        return RefInvContainers.FirstOrDefault(container => container.selfObjName == refObjName);
    }
}

[System.Serializable]
public class RefrigeratorInventoryContainerBase
{
    // TODO: 冷蔵庫判別にオブジェクト名を使用しているが、同一名称が使われる恐れがあるため、できれば他の方法に切り替えたい
    [SerializeField, Tooltip("個々の冷蔵庫オブジェクトの名前")]
    public string selfObjName;

    [SerializeField]
    private List<InventorySlotBase> container = new List<InventorySlotBase>();

    public List<InventorySlotBase> Container { get => container; private set => container = value; }

    public RefrigeratorInventoryContainerBase(string objName)
    {
        selfObjName = objName;
    }

    /// <summary>
    /// インベントリにアイテムを追加する
    /// </summary>
    /// <param name="item">追加するアイテム</param>
    /// <param name="states">アイテムの状態</param>
    public void AddItem(Item item, List<FoodState> states = null)
    {
        // スロットサイズを超過する場合は追加しない
        if (Container.Count >= RefrigeratorManager.Instance.slotSize) return;

        if (states == null)
        {
            states = new List<FoodState>() { FoodState.None };
        }

        Container.Add(new InventorySlotBase(item, states));
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
    public List<FoodState> GetState(int index)
    {
        return Container[index].States;
    }

    /// <summary>
    /// 指定したインデックスのスロットのアイテムを更新する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <param name="item">更新アイテム</param>
    /// <param name="states">アイテムの状態</param>
    public void UpdateItem(int index, Item item, List<FoodState> states)
    {
        if (Container.Count < index)
        {
            Debug.LogError("UpdateItem: Out of range");

            return;
        }

        Container[index].UpdateSlot(item, states);
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
