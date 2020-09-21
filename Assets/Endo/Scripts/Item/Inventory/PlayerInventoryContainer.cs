﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Inventory", menuName = "Inventory/Create Player Inventory")]
public class PlayerInventoryContainer : InventoryContainerBase
{
    public override void AddSlot(Item item, IEnumerable<FoodState> states)
    {
        // インベントリサイズ以上は追加しない（暫定）
        if (Container.Count >= 3) return;

        base.AddSlot(item, states);
    }

    // TODO: 投げ餌専用の管理
}
