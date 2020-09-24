using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Negoto Data", menuName = "Create Negoto Data")]
public class NegotoData : ScriptableObject
{
    [SerializeField]
    private List<NegotoEntry> entries = new List<NegotoEntry>();

    public List<NegotoEntry> Entries => entries;

    /// <summary>
    /// 寝言データを追加する
    /// </summary>
    /// <param name="requireFood">寝言で必須となっている食材</param>
    /// <param name="requireStates">寝言で必須となっている状態</param>
    /// <param name="isActive">寝言が有効か否か</param>
    public void AddEntry(int index, Item requireFood, IEnumerable<FoodState> requireStates, string contentText, bool isActive)
    {
        Entries.Add(new NegotoEntry(index, requireFood, requireStates, contentText, isActive));
    }

    /// <summary>
    /// 指定したインデックの寝言データを更新する
    /// </summary>
    /// <param name="index">更新する寝言データのインデックス</param>
    /// <param name="requireFood">寝言で必須となっている食材</param>
    /// <param name="requireStates">寝言で必須となっている状態</param>
    /// <param name="isActive">寝言が有効か否か</param>
    public void UpdateEntry(int index, Item requireFood, IEnumerable<FoodState> requireStates, string contentText, bool isActive)
    {
        // 状態がnullの場合は初期化して更新する
        if (requireStates == null)
        {
            requireStates = new List<FoodState>();
        }

        Entries.First(e => e.SelfIndex == index)
               ?.UpdateEntry(requireFood, requireStates, contentText, isActive);
    }
}

[System.Serializable]
public class NegotoEntry
{
    [SerializeField]
    private int selfIndex;

    [SerializeField]
    private Item requireFood;

    [SerializeField]
    private List<FoodState> requireStates;

    [SerializeField]
    private string contentText;

    [SerializeField]
    private bool isActive;

    public NegotoEntry(int selfIndex, Item requireFood, IEnumerable<FoodState> requireStates, string contentText, bool isActive)
    {
        SelfIndex     = selfIndex;
        RequireFood   = requireFood;
        RequireStates = requireStates.ToList();
        ContentText   = contentText;
        IsActive      = isActive;
    }

    public int             SelfIndex     { get => selfIndex;     private set => selfIndex = value; }
    public Item            RequireFood   { get => requireFood;   private set => requireFood = value; }
    public List<FoodState> RequireStates { get => requireStates; private set => requireStates = value; }
    public string          ContentText   { get => contentText;   private set => contentText = value; }
    public bool            IsActive      { get => isActive;      private set => isActive = value; }

    public void UpdateEntry(Item requireFood, IEnumerable<FoodState> requireStates, string contentText, bool isActive)
    {
        RequireFood   = requireFood;
        RequireStates = requireStates.ToList();
        ContentText   = contentText;
        IsActive      = isActive;
    }
}
