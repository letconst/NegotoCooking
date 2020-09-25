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
    public bool                    IsFull    => Container.Count(slot => slot.Item != null) == slotSize;

    /// <summary>
    /// インベントリにスロットを追加する
    /// </summary>
    /// <param name="item">追加するアイテム</param>
    /// <param name="states">アイテムの状態</param>
    public virtual void AddSlot(Item item, IEnumerable<FoodState> states = null)
    {
        // スロットサイズを超過する場合は追加しない
        if (Container.Count >= SlotSize) return;

        if (states == null)
        {
            states = new List<FoodState>()
            {
                FoodState.None
            };
        }

        Container.Add(new InventorySlotBase(item, states));
    }

    /// <summary>
    /// 一番頭に近い空きスロットにアイテムを配置する
    /// </summary>
    /// <param name="item">配置するアイテム</param>
    /// <param name="states">配置するアイテムの状態のList</param>
    public virtual void AddItem(Item item, IEnumerable<FoodState> states)
    {
        foreach (var slot in Container.Where(slot => slot.Item == null))
        {
            slot.UpdateSlot(item, states.ToList());

            break;
        }
    }

    /// <summary>
    /// 指定したインデックスのスロットアイテムを取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns>スロットのアイテム</returns>
    public virtual Item GetItem(int index)
    {
        return Container[index].Item;
    }

    /// <summary>
    /// 指定したインデックスのスロットアイテムの状態を取得する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <returns>付加されている状態のリスト</returns>
    public virtual List<FoodState> GetStates(int index)
    {
        return Container[index].States;
    }

    /// <summary>
    /// 指定したインデックスのスロットのアイテムを更新する
    /// </summary>
    /// <param name="index">スロットのインデックス</param>
    /// <param name="item">更新アイテム</param>
    /// <param name="states">アイテムの状態</param>
    public virtual void UpdateItem(int index, Item item, IEnumerable<FoodState> states)
    {
        if (Container.Count < index)
        {
            Debug.LogError("UpdateItem: Out of range");

            return;
        }

        Container[index].UpdateSlot(item, states.ToList());
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
    protected List<FoodState> states = new List<FoodState>();

    public int             ID     { get => id;     protected set => id = value; }
    public Item            Item   { get => item;   protected set => item = value; }
    public List<FoodState> States { get => states; protected set => states = value; }

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
        Item = null;
        States.Add(FoodState.Raw);
    }

    public InventorySlotBase(Item item, IEnumerable<FoodState> states = null)
    {
        if (states == null)
        {
            states = new List<FoodState>()
            {
                FoodState.None
            };
        }

        Item   = item;
        States = states.ToList();
    }

    public void UpdateSlot(Item item, IEnumerable<FoodState> states = null)
    {
        if (states == null)
        {
            states = new List<FoodState>()
            {
                FoodState.None
            };
        }

        Item   = item;
        States = states.ToList();
    }

    /// <summary>
    /// スロットのアイテムに状態を付加する
    /// 状態が重複する場合は付加されない
    /// </summary>
    /// <param name="state">付加する状態</param>
    public void AddState(FoodState state)
    {
        // None状態は除去
        States.RemoveAll(s => s == FoodState.None);

        // 状態の重複は弾く
        if (States.Contains(state)) return;

        States.Add(state);
    }

    /// <summary>
    /// スロットのアイテムから指定した状態を除去する
    /// </summary>
    /// <param name="state">除去する状態</param>
    public void RemoveState(FoodState state)
    {
        States.RemoveAll(s => s == state);
    }

    /// <summary>
    /// スロットのクローンを作成する
    /// </summary>
    /// <returns>スロットのクローン</returns>
    public InventorySlotBase DeepCopy()
    {
        return new InventorySlotBase
        {
            id     = id,
            item   = item,
            states = states
        };
    }
}

[System.Serializable]
public class DefaultItems
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private List<FoodState> state = new List<FoodState>();

    public Item            Item  => item;
    public List<FoodState> State => state;
}
