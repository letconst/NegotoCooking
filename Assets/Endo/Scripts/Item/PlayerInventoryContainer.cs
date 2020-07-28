using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodState
{
    Raw,    // 生
    Cooked, // 調理済み
    Burnt   // 焦げてる
}

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "Create Player Inventory")]
public class PlayerInventoryContainer : ScriptableObject
{
    [SerializeField]
    private List<PlayerInventorySlotObject> _container = new List<PlayerInventorySlotObject>();

    public List<PlayerInventorySlotObject> Container { get => _container; private set => _container = value; }

    /// <summary>
    /// インベントリにアイテムを追加する
    /// </summary>
    /// <param name="item"><追加するアイテム/param>
    /// <param name="state">アイテムの状態</param>
    public void AddItem(Item item, FoodState state)
    {
        if (Container.Count >= 3) return;

        Container.Add(new PlayerInventorySlotObject(item, state));
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

    // TODO: 投げ餌専用の管理
}

[System.Serializable]
public class PlayerInventorySlotObject
{
    [SerializeField]
    private int _id;

    [SerializeField]
    private Item _item;

    [SerializeField]
    private FoodState _state;

    public int       ID    { get => _id;    private set => _id    = value; }
    public Item      Item  { get => _item;  private set => _item  = value; }
    public FoodState State { get => _state; private set => _state = value; }

    public PlayerInventorySlotObject()
    {
        Item  = null;
        State = FoodState.Raw;
    }

    public PlayerInventorySlotObject(Item item, FoodState state = FoodState.Raw)
    {
        Item  = item;
        State = state;
    }

    public void UpdateSlot(Item item, FoodState state = FoodState.Raw)
    {
        Item  =  item;
        State = state;
    }

    public void ChangeState(FoodState state) => State = state;
}
