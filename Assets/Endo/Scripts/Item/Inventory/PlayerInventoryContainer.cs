using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "Inventory/Create Player Inventory")]
public class PlayerInventoryContainer : InventoryContainerBase
{
    [SerializeField]
    private int maxDogFoodCount = 4;
    [SerializeField]
    private int dogFoodCount;

    public int DogFoodCount 
    { 
        get => dogFoodCount;
        set => dogFoodCount = Mathf.Clamp(value, 0, MaxDogFoodCount);
    }
    public int MaxDogFoodCount { get => maxDogFoodCount; }

    public override void AddItem(Item item, List<FoodState> states)
    {
        // インベントリサイズ以上は追加しない（暫定）
        if (Container.Count >= 3) return;

        base.AddItem(item, states);
    }

    // TODO: 投げ餌専用の管理
}


//餌用の持たせる領域の確保