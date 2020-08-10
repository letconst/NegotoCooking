using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodState
{
    None,   // なし
    Raw,    // 生
    Cooked, // 調理済み
    Burnt,   // 焦げてる
    Cut,   // 切れている
    Boil,   // 煮込んである
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Create Inventory")]
public class InventoryContainerBase : ScriptableObject
{
    [SerializeField, Tooltip("スロットの最大数（初期値：3）")]
    private int _slotSize = 3;

    [SerializeField]
    private List<InventorySlotBase> _container = new List<InventorySlotBase>();

    public List<InventorySlotBase> Container { get => _container; protected set => _container = value; }
    public int                     SlotSize  { get => _slotSize; protected set => _slotSize = value; }

    /// <summary>
    /// インベントリにアイテムを追加する
    /// </summary>
    /// <param name="item"><追加するアイテム/param>
    /// <param name="state">アイテムの状態</param>
    public virtual void AddItem(Item item, FoodState state = FoodState.None)
    {
        // スロットサイズを超過する場合は追加しない
        if (Container.Count >= SlotSize) return;

        Container.Add(new InventorySlotBase(item, state));
    }

    /// <summary>
    /// 指定したインデックスのスロットアイテムを取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns></returns>
    public virtual Item GetItem(int index)
    {
        return Container[index].Item;
    }

    /// <summary>
    /// 指定したインデックスのスロットアイテムの状態を取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns></returns>
    public virtual FoodState GetState(int index)
    {
        return Container[index].State;
    }

    /// <summary>
    /// 指定したインデックスのスロットのアイテムを更新する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <param name="item">更新アイテム</param>
    /// <param name="state">アイテムの状態</param>
    public virtual void UpdateItem(int index, Item item, FoodState state)
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
    public virtual void RemoveItem(int index)
    {
        if (Container.Count < index)
        {
            Debug.LogError("RemoveItem: Out of range");
            return;
        }

        Container[index].UpdateSlot(null);
    }
}

[System.Serializable]
public class InventorySlotBase
{
    [SerializeField]
    protected int _id;

    [SerializeField]
    protected Item _item;

    [SerializeField]
    protected FoodState _state;

    public int       ID    { get => _id;    protected set => _id    = value; }
    public Item      Item  { get => _item;  protected set => _item  = value; }
    public FoodState State { get => _state; protected set => _state = value; }

    public InventorySlotBase()
    {
        Item  = null;
        State = FoodState.Raw;
    }

    public InventorySlotBase(Item item, FoodState state = FoodState.Raw)
    {
        Item  = item;
        State = state;
    }

    public void UpdateSlot(Item item, FoodState state = FoodState.Raw)
    {
        Item = item;
        State = state;
    }

    public void ChangeState(FoodState state) => State = state;
}
