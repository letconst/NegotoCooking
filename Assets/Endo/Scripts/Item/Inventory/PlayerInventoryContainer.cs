using UnityEngine;

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "Inventory/Create Player Inventory")]
public class PlayerInventoryContainer : InventoryContainerBase
{
    public override void AddItem(Item item, FoodState state)
    {
        // インベントリサイズ以上は追加しない（暫定）
        if (Container.Count >= 3) return;

        base.AddItem(item, state);
    }
    // TODO: 投げ餌専用の管理
}
