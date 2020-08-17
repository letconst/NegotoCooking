using System.Collections;
using System.Collections.Generic;
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
    /// <param name="refObj">追加する冷蔵庫のオブジェクト</param>
    public void AddContainer(GameObject refObj)
    {
        // 同じインスタンスIDのコンテナがあれば追加しない
        foreach (var container in RefInvContainers)
        {
            if (container._selfInstanceId == refObj.GetInstanceID()) return;
        }

        RefInvContainers.Add(new RefrigeratorInventoryContainerBase(refObj.GetInstanceID()));
    }

    /// <summary>
    /// 指定したインスタンスIDの冷蔵庫オブジェクトのコンテナを返す
    /// 指定IDに一致するコンテナが見つからない場合はnullを返す
    /// </summary>
    /// <param name="instanceId">取得したいコンテナの冷蔵庫オブジェクトのインスタンスID</param>
    /// <returns>コンテナ || null</returns>
    public RefrigeratorInventoryContainerBase GetContainer(int instanceId)
    {
        RefrigeratorInventoryContainerBase result = null;

        foreach (var container in RefInvContainers)
        {
            if (container._selfInstanceId == instanceId)
            {
                result = container;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 指定したインスタンスIDの冷蔵庫オブジェクトのコンテナインデックスを返す
    /// 見つからない場合は-1を返す
    /// </summary>
    /// <param name="instanceId">取得したいコンテナの冷蔵庫オブジェクトのインスタンスID</param>
    /// <returns>コンテナのインデックス || -1</returns>
    public int GetContainerIndex(int instanceId)
    {
        int result = -1;
        int i      = 0;

        foreach (var container in RefInvContainers)
        {
            if (container._selfInstanceId == instanceId)
            {
                result = i;
                break;
            }

            i++;
        }

        return result;
    }
}

[System.Serializable]
public class RefrigeratorInventoryContainerBase
{
    [SerializeField, Tooltip("個々の冷蔵庫オブジェクトのインスタンスID")]
    public int _selfInstanceId;

    [SerializeField]
    private List<InventorySlotBase> _container = new List<InventorySlotBase>();

    public List<InventorySlotBase> Container { get => _container; private set => _container = value; }

    public RefrigeratorInventoryContainerBase(int instanceId)
    {
        _selfInstanceId = instanceId;
    }

    /// <summary>
    /// インベントリにアイテムを追加する
    /// </summary>
    /// <param name="item"><追加するアイテム/param>
    /// <param name="state">アイテムの状態</param>
    public void AddItem(Item item, FoodState state = FoodState.None)
    {
        // スロットサイズを超過する場合は追加しない
        if (Container.Count >= RefrigeratorManager.Instance.slotSize) return;

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
