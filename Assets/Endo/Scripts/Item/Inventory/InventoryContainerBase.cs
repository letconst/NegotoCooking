using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FoodState
{
    None,   // なし
    Raw,    // 生
    Cooked, // 調理済み
    Burnt,  // 焦げてる
    Cut,    // 切れている
    Boil,   // 煮込んである
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Create Inventory")]
public class InventoryContainerBase : ScriptableObject
{
    [SerializeField, Tooltip("スロットの最大数（初期値：3）")]
    private int slotSize = 3;

    [SerializeField]
    private List<InventorySlotBase> container = new List<InventorySlotBase>();

    public List<InventorySlotBase> Container { get => container; protected set => container = value; }
    public int                     SlotSize  { get => slotSize;  protected set => slotSize = value; }

    /// <summary>
    /// インベントリにアイテムを追加する
    /// </summary>
    /// <param name="item">追加するアイテム</param>
    /// <param name="states">アイテムの状態</param>
    public virtual void AddItem(Item item, HashSet<FoodState> states = null)
    {
        // スロットサイズを超過する場合は追加しない
        if (Container.Count >= SlotSize) return;

        if (states == null)
        {
            states = new HashSet<FoodState>() { FoodState.None };
        }

        Container.Add(new InventorySlotBase(item, states));
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
    public virtual HashSet<FoodState> GetState(int index)
    {
        return Container[index].States;
    }

    /// <summary>
    /// 指定したインデックスのスロットのアイテムを更新する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <param name="item">更新アイテム</param>
    /// <param name="states">アイテムの状態</param>
    public virtual void UpdateItem(int index, Item item, HashSet<FoodState> states)
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
    protected int id;

    [SerializeField]
    protected Item item;

    [SerializeField]
    protected HashSet<FoodState> states;

    public int       ID     { get => id;    protected set => id = value; }
    public Item      Item   { get => item;  protected set => item = value; }
    public HashSet<FoodState> States { get => states; protected set => states = value; }

    public string FullItemName
    {
        get
        {
            var result = "";

            if (Item == null) return result;

            switch (States.ElementAt(0))
            {
                case FoodState.None:
                    if (Item             != null &&
                        Item.KindOfItem1 != Item.KindOfItem.Seasoning)
                    {
                        Debug.LogWarning($"アイテム「{Item.ItemName}」の状態がNoneです。アイテムを持たせる際は適切な状態を指定してください");
                    }

                    result = Item.ItemName;

                    break;

                case FoodState.Raw:
                    result = Item.ItemName;

                    break;

                case FoodState.Cooked:
                    result = $"焼いた{Item.ItemName}";

                    break;

                case FoodState.Burnt:
                    result = $"焦げた{Item.ItemName}";

                    break;

                case FoodState.Boil:
                    result = $"煮込んだ{Item.ItemName}";

                    break;

                case FoodState.Cut:
                    result = $"切った{Item.ItemName}";

                    break;

                default:
                    Debug.LogWarning($"状態「{States}」の接頭辞が未設定です");

                    result = Item.ItemName;

                    break;
            }

            return result;
        }
    }

    public InventorySlotBase()
    {
        Item  = null;
        States.Add(FoodState.Raw);
    }

    public InventorySlotBase(Item item, HashSet<FoodState> states = null)
    {
        if (states == null)
        {
            states = new HashSet<FoodState>() { FoodState.None };
        }

        Item  = item;
        States = states;
    }

    public void UpdateSlot(Item item, HashSet<FoodState> states = null)
    {
        if (states == null)
        {
            states = new HashSet<FoodState>() { FoodState.None };
        }

        Item  = item;
        States = states;
    }

    public void ChangeState(FoodState state) => States.Add(state);
}

[System.Serializable]
public class DefaultItems
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private HashSet<FoodState> state;

    public Item      Item  => item;
    public HashSet<FoodState> State => state;
}
